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

namespace ArchaismDictionaryAndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, CameraSource.IPictureCallback
    {
        #region Variables 

        #region MainScreenVariables
        private SurfaceView cameraView;
        private CameraSource cameraSource;
        private ImageView freezeFrameImage;
        private Button captureButton;
        private TextView resultText;
        private Button unfreezeButton;
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

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            AssignUIVariables();
            CreateGoogleCredentials();
            CreateCameraSource();
        }
        
        #region UIManager
    
        private void AssignUIVariables()
        {
            cameraView = FindViewById<SurfaceView>(Resource.Id.cameraView);
            freezeFrameImage = FindViewById<ImageView>(Resource.Id.freezeframeView);
            captureButton = FindViewById<Button>(Resource.Id.captureButton);
            resultText = FindViewById<TextView>(Resource.Id.resultText);
            unfreezeButton = FindViewById<Button>(Resource.Id.unfreezeButton);

            captureButton.Click += TakePicture;
            unfreezeButton.Click += UnfreezeFrame;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            resultText.Enabled = false;
            resultText.Alpha = 0;
        }

        private void TakePicture(object sender, EventArgs e)
        {
            cameraSource.TakePicture(null, this);
        }

        private void UnfreezeFrame(object sender, EventArgs e)
        {
            captureButton.Enabled = true;
            cameraView.Enabled = true;

            unfreezeButton.Enabled = false;
            unfreezeButton.Alpha = 0;
            resultText.Enabled = false;
            unfreezeButton.Alpha = 0;
            freezeFrameImage.Enabled = false;
            unfreezeButton.Alpha = 0;
        }

        private void FreezeFrame(Bitmap bitmap)
        {
            freezeFrameImage.SetImageBitmap(bitmap);
            freezeFrameImage.Enabled = true;

            cameraView.Enabled = false;
            cameraView.Alpha = 0;
            captureButton.Enabled = false;
            captureButton.Alpha = 0;
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
            if (response != null)
            {
                result = "";
                foreach (var annotation in response)
                {
                    if (annotation.Description != null)
                    {
                        result += annotation.Description + "\r\n";
                    }
                }
            }

            return result;
        }

        public void OnPictureTaken(byte[] data)
        {
            Bitmap loadedImage = null;
            Bitmap bitmap = null;

            loadedImage = BitmapFactory.DecodeByteArray(data, 0,
                    data.Length);

            Matrix rotateMatrix = new Matrix();
            rotateMatrix.PostRotate(90f);
            bitmap = Bitmap.CreateBitmap(loadedImage, 0, 0, loadedImage.Width, loadedImage.Height, rotateMatrix, false);

            result = OCR(data);
            resultText.Text = result;
            FreezeFrame(bitmap);
        }
        #endregion
    }
}