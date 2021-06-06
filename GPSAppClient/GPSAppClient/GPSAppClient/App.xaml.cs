using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GPSAppClient
{
	public partial class App : Application
	{
        public static NavigationPage MyNavigationPage;
        public App ()
		{
			InitializeComponent();
            MyNavigationPage = new NavigationPage();
            MainPage = MyNavigationPage;
            MyNavigationPage.PushAsync(new StartPage(), true);
            //MainPage = new GPSAppClient.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
