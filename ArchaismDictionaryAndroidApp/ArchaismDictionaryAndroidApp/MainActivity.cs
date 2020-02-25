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

namespace ArchaismDictionaryAndroidApp
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/AppLogo", Theme = "@style/splash", MainLauncher = true)]
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
        private ImageView loadingImage;
        private TextView loadingText;
        private Android.Support.Design.Internal.BottomNavigationItemView bottomBarDict;
        private Android.Support.Design.Internal.BottomNavigationItemView bottomBarOCR;
        #endregion

        #region SecondaryScreenVariables
        ImageView searchTopBackground;
        EditText searchInput;
        TextView searchWord;
        TextView searchDefinition;
        Button search;
        #endregion

        #region GoogleCredentialVariables
        GoogleCredential credentials;
        StorageClient storage;
        Grpc.Core.Channel channel;
        #endregion

        #region OCRVariables
        private const int requestCameraPermission = 1001;
        public string result;
        #endregion

        #region NetworkVariables
        public static string[,] dataBase = { { "Заптие", "Войник от турско време" }, { "Игото", "Робството" }, {"i", "beeboop"} };
        public bool isConnected { get; set; }
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.splash);
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
            loadingImage = FindViewById<ImageView>(Resource.Id.loadingImage);
            loadingText = FindViewById<TextView>(Resource.Id.loadingText);

            bottomBarOCR = FindViewById<Android.Support.Design.Internal.BottomNavigationItemView>(Resource.Id.navigation_ocr);
            bottomBarDict = FindViewById<Android.Support.Design.Internal.BottomNavigationItemView>(Resource.Id.navigation_dictionary);

            searchTopBackground = FindViewById<ImageView>(Resource.Id.topBackground);
            searchInput = FindViewById<EditText>(Resource.Id.dictionaryInput);
            searchWord = FindViewById<TextView>(Resource.Id.dictionaryWord);
            searchDefinition = FindViewById<TextView>(Resource.Id.dictionaryDefinition);
            search = FindViewById<Button>(Resource.Id.searchButton);

            NotLoading();

            captureButton.Click += TakePicture;
            unfreezeButton.Click += UnfreezeFrame;
            retry.Click += RemoveErrorScreen;
            bottomBarDict.Click += SearchScreen;
            bottomBarOCR.Click += OCRScreenButton;
            search.Click += Search;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            freezeFrameImage.Enabled = false;
            resultText.Enabled = false;
            resultText.Alpha = 0;
            errorScreen.Enabled = false;
            errorScreen.Alpha = 0;
            retry.Enabled = false;
            retry.Alpha = 0;

            OCRScreen();
        }

        private void TakePicture(object sender, EventArgs e)
        {
            LoadingScreen();
            cameraSource.TakePicture(null, this);
        }

        private void Search(object sender, EventArgs e)
        {
            searchDefinition.Text = FindWordInDictionary(searchInput.Text);
            string[] arr = searchDefinition.Text.Split(":", 2);
            searchWord.Text = arr[0];
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

        private void FreezeFrame(Bitmap bitmap)
        {
            freezeFrameImage.Enabled = true;
            freezeFrameImage.SetImageBitmap(Bitmap.CreateScaledBitmap(bitmap, bitmap.Width, (int)(bitmap.Height * 1.05f), false));
            freezeFrameImage.Alpha = 256;

            unfreezeButton.Enabled = true;
            unfreezeButton.Alpha = 256;

            cameraView.Alpha = 0;
            freezeFrameImage.SetMinimumHeight(cameraView.Height);
            cameraView.Enabled = false;
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

        private void LoadingScreen()
        {
            loadingText.Enabled = true;
            loadingImage.Enabled = true;
            loadingText.Alpha = 256;
            loadingImage.Alpha = 256;
        }

        private void NotLoading()
        {
            loadingText.Enabled = false;
            loadingImage.Enabled = false;
            loadingText.Alpha = 0;
            loadingImage.Alpha = 0;
        }

        private void OCRScreenButton(object sender, EventArgs e)
        {
            OCRScreen();
        }

        private void OCRScreen()
        {
            searchTopBackground.Enabled = false;
            searchTopBackground.Alpha = 0;
            searchInput.Enabled = false;
            searchInput.Alpha = 0;
            searchWord.Enabled = false;
            searchWord.Alpha = 0;
            searchDefinition.Enabled = false;
            searchDefinition.Alpha = 0;
            search.Enabled = false;
            search.Alpha = 0;

            cameraView.Enabled = true;
            cameraView.Alpha = 256;
            captureButton.Enabled = true;
            captureButton.Alpha = 256;
        }

        private void SearchScreen(object sender, EventArgs e)
        {
            searchTopBackground.Enabled = true;
            searchTopBackground.Alpha = 256;
            searchInput.Enabled = true;
            searchInput.Alpha = 256;
            searchWord.Enabled = true;
            searchWord.Alpha = 256;
            searchDefinition.Enabled = true;
            searchDefinition.Alpha = 256;
            search.Enabled = true;
            search.Alpha = 256;

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
            errorScreen.Enabled = false;
            errorScreen.Alpha = 0;
            retry.Enabled = false;
            retry.Alpha = 0;
            loadingImage.Enabled = false;
            loadingImage.Alpha = 0;
            loadingText.Enabled = false;
            loadingText.Alpha = 0;
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
            var img = Image.FromBytes(bytes);
            var response = client.DetectText(img);

            result = string.Empty;

            if (response != null)
            {
                foreach (var annotation in response)
                {
                    if (annotation.Description != null)
                    {
                        result = FindWordInDictionary(annotation.Description);
                    }
                }
            }

            return result;
        }

        public void OnPictureTaken(byte[] data)
        {
            NotLoading();
            CheckConnection();

            if (isConnected == true)
            {
                result = OCR(data);

                if (result != string.Empty)
                {
                    Bitmap loadedImage;
                    Bitmap bitmap;

                    loadedImage = BitmapFactory.DecodeByteArray(data, 0, data.Length);
                    Matrix rotateMatrix = new Matrix();
                    rotateMatrix.PostRotate(90f);
                    bitmap = Bitmap.CreateBitmap(loadedImage, 0, 0, loadedImage.Width, loadedImage.Height, rotateMatrix, false);

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
        public static string FindWordInDictionary(string input)
        {
            string final = string.Empty;

            input = input.ToLower();

            string[] inputWords = input.Split("\n");

            foreach (string word in inputWords)
            {
                for (int i = 0; i < dataBase.Length / 2; i++)
                {
                    if (word == dataBase[i, 0])
                    {
                        final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                    }
                    else
                    {
                        if (word.Length > 4 && dataBase[i, 0].Length > 4)
                        {
                            if (word.Length < dataBase[i, 0].Length)
                            {
                                if (word.Contains(dataBase[i, 0]) == true)
                                {
                                    final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                                }
                            }
                            else
                            {
                                if (dataBase[i, 0].Contains(word) == true)
                                {
                                    final = dataBase[i, 0].First().ToString().ToUpper() + String.Join("", dataBase[i, 0].Skip(1)) + ":\n" + dataBase[i, 1].First().ToString().ToUpper() + String.Join("", dataBase[i, 1].Skip(1)) + ".";
                                }
                            }
                        }
                    }
                }
            }
            return final;
        }
        #endregion

    }
}