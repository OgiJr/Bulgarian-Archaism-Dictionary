using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Archaism_Dictionary_App.Services;
using Archaism_Dictionary_App.Views;

namespace Archaism_Dictionary_App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
