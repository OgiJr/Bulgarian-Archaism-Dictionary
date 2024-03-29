﻿using Android;
using Android.App;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ArchaismDictionaryAndroidApp.Network;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ArchaismDictionaryAndroidApp
{
    [Activity(Label = "Арх-Речник", Icon = "@drawable/AppLogo", Theme = "@style/splash", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISurfaceHolderCallback, CameraSource.IPictureCallback, INetworkConnection
    {
        #region Variables 

        /// <summary>
        ///  UI elements which are displayed on the OCR page
        /// </summary>
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
        /// <summary>
        ///  UI elements which are displayed on the Search page
        ///  /// </summary>
        #region SecondaryScreenVariables
        private ImageView searchTopBackground;
        private EditText searchInput;
        private TextView searchWord;
        private TextView searchDefinition;
        #endregion

        #region GoogleCredentialVariables
        private GoogleCredential credentials;
        private StorageClient storage;
        private Grpc.Core.Channel channel;
        #endregion

        #region OCRVariables
        private const int requestCameraPermission = 1001;
        private string result;
        ImageAnnotatorClient client;
        #endregion

        #region NetworkVariables
        /// <summary>
        /// A matrix string [wordName, wordDefinition]
        /// </summary>
        private static string[,] dataBase;
        public bool isConnected { get; set; }
        /// <summary>
        /// How many words there are in the JSON file
        /// </summary>
        private int wordCount;
        private object context;
        #endregion

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.SetTheme(Resource.Style.splash);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            CheckConnection();
            AssignUIVariables();

            if (isConnected == true)
            {
                DictionaryManager();
                CreateGoogleCredentials();
                CreateCameraSource();
                client = ImageAnnotatorClient.Create(channel);
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

        /// <summary>
        /// Displays a UI error screen
        /// </summary>
        private void NetworkErrorScreen()
        {
            ErrorScreen();
            errorScreen.Text = "Моля свържете се към интернет";
        }
        #endregion

        #region UIManager
        /// <summary>
        /// Assigns variable to XML counterpart
        /// </summary>
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

            NotLoading();

            captureButton.Click += TakePicture;
            unfreezeButton.Click += UnfreezeFrame;
            retry.Click += RemoveErrorScreen;
            bottomBarDict.Click += SearchScreen;
            bottomBarOCR.Click += OCRScreenButton;
            searchInput.EditorAction += (_, e) =>
            {
                if (e.ActionId == ImeAction.Done)
                {
                    Search();
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(searchInput.ApplicationWindowToken, 0);
                }
            };

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

        /// <summary>
        /// When the user requests to take a picture via the user interface
        /// </summary>
        private void TakePicture(object sender, EventArgs e)
        {
            LoadingScreen();
            cameraSource.TakePicture(null, this);
        }

        private void Search()
        {
            if (searchInput.Text != string.Empty)
            {
                string result = FindWordInDictionary(searchInput.Text);
                string[] arr = result.Split(":", 2);
                searchDefinition.Text = arr[1];
                searchWord.Text = arr[0];
            }
        }

        /// <summary>
        /// This section deals with the UI after detecting a word from the dictionary or when the user wants to go back and scan another word.
        /// </summary>
        #region ScanningUI
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
            freezeFrameImage.Alpha = 0;
            unfreezeButton.Alpha = 0;
            resultText.Enabled = false;
            resultText.Alpha = 0;
        }

        /// <summary>
        /// This function is called by the TakePicture method from the main activity. This function freezes the camera frame
        /// </summary>
        /// <param name="bitmap">The camera frame which Google Cloud Vision gets</param>s
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
        #endregion

        /// <summary>
        /// Turns on and off the error screen when there is an error.
        /// </summary>
        #region ErrorUI
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
            DictionaryManager();

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
        #endregion

        /// <summary>
        /// Called from the main activity when the application needs to load
        /// </summary>
        #region LoadingScreen
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
        #endregion

        /// <summary>
        /// Assigns each of the buttons from the bottom bar navigation a corresponding function, to change between the main screens (OCR, Search, etc).
        /// </summary>
        #region BottomBarNavigation
        private void OCRScreenButton(object sender, EventArgs e)
        {
            OCRScreen();
        }

        private void OCRScreen()
        {

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

        #endregion

        #region Credentials

        /// <summary>
        /// Requests the usage of the camera by the user from the manifest file
        /// </summary>
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

        /// <summary>
        /// Creates the credentials that are needed for Cloud vision to operate
        /// </summary>
        public void CreateGoogleCredentials()
        {
            string path = "44fe2d26dab6.json";
            Stream stream = Application.Context.Assets.Open(path);

            credentials = GoogleCredential.FromStream(stream);
            storage = StorageClient.Create(credentials);
            channel = new Grpc.Core.Channel(ImageAnnotatorClient.DefaultEndpoint.ToString(), credentials.ToChannelCredentials());
        }
        #endregion

        //Creates the camera source and connects it to the user interface.
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
        /// <summary>
        /// References Cloud Vision and through it recognizes the optical characters
        /// </summary>
        /// <param name="bytes">The image input</param>
        /// <returns></returns>
        private async Task<string> OCR(byte[] bytes)
        {
            var img = Image.FromBytes(bytes);
            var response = client.DetectText(img);

            result = string.Empty;

            if (response != null)
            {
                foreach (var annotation in response)
                {
                    if (annotation.Description != null && result == string.Empty)
                    {
                        result = FindWordInDictionary(annotation.Description);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Starts scanning the picture which is requested from the TakePicture method
        /// </summary>
        public void OnPictureTaken(byte[] data)
        {
            NotLoading();
            CheckConnection();

            if (isConnected == true)
            {
                result = OCR(data).Result;

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
        /// <summary>
        /// Searches for a word inside of the Network manager's database matrix
        /// </summary>
        /// <param name="input">This is the input word which is searched for inside of the matrix</param>
        /// <returns></returns>
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
                }
            }

            if (final == string.Empty)
            {
                final = input + ":\nНе намерихме вашата дума в речника ни. \nМоже да я добавите през нашия уебсайт.";
            }

            return final;
        }

        //public static string FindWordInDictionaryText(string input)
        //{

        //}

        /// <summary>
        /// Reads the JSON file from the internet and assigns its values to the dictionary matrix
        /// </summary>
        private void DictionaryManager()
        {
            string rawJSON;
            const string fileName = "dictionary.json";
            string jsonRead;

            WebClient client = new WebClient();
            jsonRead = client.DownloadString("http://archaismdictionary.bg/json_manager.php"); ;

            File.WriteAllText(FileSystem.AppDataDirectory + fileName, jsonRead);

            rawJSON = File.ReadAllText(FileSystem.AppDataDirectory + fileName);

            var list = JsonConvert.DeserializeObject<Dictionary.JSONClass>(rawJSON);

            wordCount = list.Property1[2].data.Length;

            dataBase = new string[wordCount, 2];

            for (int i = 0; i < wordCount; i++)
            {
                dataBase[i, 0] = list.Property1[2].data[i].word;
                dataBase[i, 1] = list.Property1[2].data[i].definition;
            }
        }
    }
}
#endregion