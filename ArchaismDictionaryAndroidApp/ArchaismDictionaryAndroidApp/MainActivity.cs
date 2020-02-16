using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Graphics;
using Android.Support.V4.App;
using Android;
using System.IO;
using Google.Cloud.Vision.V1;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Grpc.Auth;
using System.Linq;
using ArchaismDictionaryAndroidApp.Network;
using Android.Net;
using Android.Content;
using SkiaSharp;

namespace ArchaismDictionaryAndroidApp
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/AppLogo", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, CameraSource.IPictureCallback, INetworkConnection
    {
        #region Variables 

        #region MainScreenVariables
        private SurfaceView cameraView;
        private CameraSource cameraSource;
        private ImageView freezeFrameImage;
        private TextView resultText;
        private TextView errorScreen;
        private ImageButton captureButton;
        private ImageButton unfreezeButton;
        private Button retry;
        #endregion

        #region GoogleCredentialVariables
        GoogleCredential credentials;
        StorageClient storage;
        Grpc.Core.Channel channel;
        #endregion

        #region OCRVariables
        private const int requestCameraPermission = 1001;
        public string result;
        public SKBitmap editedImage;
        #endregion

        #region NetworkVariables
        public static string[,] dataBase = { { "архаизъм", "дума със старинен произход" }, { "игото", "робството" } };

        public bool isConnected { get; set; }
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            CheckConnection();
            AssignUIVariables();

            if (isConnected == true)
            {
                CreateGoogleCredentials();
                CreateCameraSource();
            }
            else
            {
                NetworkErrorScreen();
            }

        }

        #region NetworkManager

        public void CheckConnection()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworking = connectivityManager.ActiveNetworkInfo;

            if (activeNetworking != null && activeNetworking.IsConnected == true)
            {
                isConnected = true;
            }

            else
            {
                isConnected = false;
            }
        }

        private void NetworkErrorScreen()
        {
            ErrorScreen();
            errorScreen.Text = "Моля свържете се към интернет";
        }

        #endregion

        #region UIManager

        private void AssignUIVariables()
        {
            cameraView = FindViewById<SurfaceView>(Resource.Id.cameraView);
            freezeFrameImage = FindViewById<ImageView>(Resource.Id.freezeframeView);
            captureButton = FindViewById<ImageButton>(Resource.Id.captureButton);
            resultText = FindViewById<TextView>(Resource.Id.resultText);
            unfreezeButton = FindViewById<ImageButton>(Resource.Id.unfreezeButton);
            errorScreen = FindViewById<TextView>(Resource.Id.errorText);
            retry = FindViewById<Button>(Resource.Id.retry);

            captureButton.Click += TakePicture;
            unfreezeButton.Click += UnfreezeFrame;
            retry.Click += RemoveErrorScreen;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            freezeFrameImage.Enabled = false;
            resultText.Enabled = false;
            resultText.Alpha = 0;
            errorScreen.Enabled = false;
            errorScreen.Alpha = 0;
            retry.Enabled = false;
            retry.Alpha = 0;

            FontDesign();
        }

        private void FontDesign()
        {
            var appFont = Typeface.CreateFromAsset(ApplicationContext.Assets, "Roboto-Regular.ttf");
            resultText.Typeface = appFont;
            errorScreen.Typeface = appFont;
        }

        private void TakePicture(object sender, EventArgs e)
        {
            cameraSource.TakePicture(null, this);
        }

        private void UnfreezeFrame(object sender, EventArgs e)
        {
            captureButton.Enabled = true;
            captureButton.Alpha = 256;
            cameraView.Alpha = 256;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            resultText.Enabled = false;
            unfreezeButton.Alpha = 0;
            freezeFrameImage.Enabled = false;
            unfreezeButton.Alpha = 0;
            resultText.Enabled = false;
            resultText.Alpha = 0;
        }

        private void FreezeFrame(Android.Graphics.Bitmap bitmap)
        {
            freezeFrameImage.Enabled = true;
            freezeFrameImage.SetImageBitmap(bitmap);
            freezeFrameImage.Alpha = 256;

            unfreezeButton.Enabled = true;
            unfreezeButton.Alpha = 256;

            cameraView.Alpha = 0;
            captureButton.Alpha = 0;
            captureButton.Enabled = false;

            resultText.Enabled = true;
            resultText.Alpha = 256;
            resultText.Text = result;
        }

        private void ErrorScreen()
        {
            errorScreen.Enabled = true;
            errorScreen.Alpha = 256;
            retry.Enabled = true;
            retry.Alpha = 256;

            cameraView.Enabled = false;
            cameraView.Alpha = 0;
            freezeFrameImage.Enabled = false;
            freezeFrameImage.Alpha = 0;
            captureButton.Enabled = false;
            captureButton.Alpha = 0;
            resultText.Enabled = false;
            resultText.Alpha = 0;
            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
        }

        private void RemoveErrorScreen(object sender, EventArgs e)
        {
            CreateGoogleCredentials();
            CreateCameraSource();

            cameraView.Enabled = true;
            cameraView.Alpha = 256;
            captureButton.Enabled = true;
            captureButton.Alpha = 256;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            freezeFrameImage.Enabled = false;
            freezeFrameImage.Alpha = 0;
            resultText.Enabled = false;
            resultText.Alpha = 0;
            retry.Enabled = false;
            retry.Alpha = 0;
            errorScreen.Enabled = false;
            errorScreen.Alpha = 0;

        }

        private SKBitmap HighlightWord(byte[] bytes, Google.Cloud.Vision.V1.EntityAnnotation entityAnnotation)
        {
            var ms = new MemoryStream(bytes);
            
            SKBitmap bitmap = SKBitmap.Decode(ms);
            var canvas = new SKCanvas(bitmap);
            
            SKPaint skPaint = new SKPaint { Color = new SkiaSharp.SKColor(255,255,0,50)};
            SKPoint[] points =  entityAnnotation.BoundingPoly.Vertices.Select((vertex) => new SKPoint(vertex.X, vertex.Y)).ToArray();
            SKRect rect = new SKRect(points[0].Y, points[1].Y, points[2].X, points[3].X);
            
            canvas.DrawRect(rect, skPaint);

            return bitmap;
        }

        #endregion

        #region Credentials

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case requestCameraPermission:
                    if (grantResults[0] == Android.Content.PM.Permission.Granted)
                    {
                        cameraSource.Start(cameraView.Holder);
                    }
                    break;
            }
        }

        public void CreateGoogleCredentials()
        {
            string path = "d576ac9cb652.json";
            Stream stream = Application.Context.Assets.Open(path);

            credentials = GoogleCredential.FromStream(stream);
            storage = StorageClient.Create(credentials);
            channel = new Grpc.Core.Channel(ImageAnnotatorClient.DefaultEndpoint.ToString(), credentials.ToChannelCredentials());
        }
        #endregion

        #region SurfaceManager

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[]
                {
                Android.Manifest.Permission.Camera
                }, requestCameraPermission);
                return;
            }
            cameraSource.Start(cameraView.Holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void Release()
        {
        }

        public void CreateCameraSource()
        {
            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();

            if (textRecognizer.IsOperational)
            {
                cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer).
                    SetFacing(CameraFacing.Back).
                    SetRequestedPreviewSize(1280, 1024).
                    SetRequestedFps(2.0f).
                    SetAutoFocusEnabled(true)
                    .Build();

                cameraView.Holder.AddCallback(this);
                freezeFrameImage.Alpha = 0;
            }
            else
            {
                Log.Error("Main Activity", "Имаше грешка при инициализирането на вашата камера.");
            }
        }

        #endregion

        #region OCR
        private string OCR(byte[] bytes)
        {
            var client = ImageAnnotatorClient.Create(channel);
            var img = Google.Cloud.Vision.V1.Image.FromBytes(bytes);
            var response = client.DetectText(img);

            result = string.Empty;

            if (response != null)
            {
                foreach (var annotation in response)
                {
                    if (annotation.Description != null)
                    {
                        result = FindWordInDatabase(annotation.Description);

                        if (annotation.Description == result && result != string.Empty)
                        {
                            editedImage = HighlightWord(bytes, annotation);
                            break;
                        }

                    }
                }
            }

            return result;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using var ms = new MemoryStream();
            imageIn.Save(ms, imageIn.RawFormat);
            return ms.ToArray();
        }

        public void OnPictureTaken(byte[] data)
        {
            if (isConnected == true)
            {
                result = OCR(data);

                if (result != string.Empty)
                {
                    Android.Graphics.Bitmap loadedImage;
                    Android.Graphics.Bitmap bitmap;

                    byte[] newData = editedImage.Bytes;

                    loadedImage = BitmapFactory.DecodeByteArray(newData, 0, newData.Length);

                    Matrix rotateMatrix = new Matrix();
                    rotateMatrix.PostRotate(90f);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(loadedImage, 0, 0, loadedImage.Width, loadedImage.Height, rotateMatrix, false);

                    FreezeFrame(bitmap);
                }
            }
            else
            {
                NetworkErrorScreen();
            }
        }
        #endregion

        #region DictionaryManager
        public static string FindWordInDatabase(string input)
        {
            string final = string.Empty;

            input = input.ToLower();

            for (int i = 0; i < dataBase.Length / 2; i++)
            {
                if (input == dataBase[i, 0])
                {
                    final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                }
                else
                {
                    char[] wordOne = input.ToCharArray();
                    char[] wordTwo = dataBase[i, 0].ToCharArray();

                    if (wordOne.Length > 4 && wordTwo.Length > 4)
                    {
                        int difference = 0;

                        if (wordOne.Length < wordTwo.Length)
                        {
                            for (int j = 0; j < wordOne.Length; j++)
                            {
                                if (wordOne[j] != wordTwo[j])
                                {
                                    difference++;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < wordTwo.Length; j++)
                            {
                                if (wordOne[j] != wordTwo[j])
                                {
                                    difference++;
                                }
                            }
                        }

                        if (difference < 3)
                        {
                            final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                        }
                    }
                }
            }

            return final;
        }
        #endregion

    }
}