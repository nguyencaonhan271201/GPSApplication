using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Connectivity;
using Xamarin.Forms;
using Android;
using Android.Locations;
using Android.Content;
using Android.Net;


namespace GPSAppClient
{
    public partial class StartPage : ContentPage
    {
        bool gpscheck, netcheck, switchstate;
        Xamarin.Forms.IButtonController t;
        public StartPage()
        {
            InitializeComponent();
            t = btnSwitch as IButtonController;
            switchstate = false;
            lblGPSCheck.IsVisible = false;
            btnSwitch.Text = "Kiểm tra lại";
            btnSwitch.IsVisible = true;
            btnSwitch.Clicked += BtnSwitch_Clicked;
            OverallCheck();
        }

        private void BtnSwitch_Clicked(object sender, EventArgs e)
        {
            switch (switchstate)
            {
                case true:
                    App.MyNavigationPage.PushAsync(new LoginPage(), true);
                    break;
                case false:
                    OverallCheck();
                    break;
            }
        }

        public static bool CheckNetwork()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        private async void Check()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            await RetrieveLocation();
        }

        private async Task RetrieveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            try
            {
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
                gpscheck = true;
            }
            catch
            {
                gpscheck = false;
                t.SendClicked();
            }
        }

        private void OverallCheck()
        {
            netcheck = CheckNetwork();
            //Check();
            /*switch (gpscheck)
            {
                case true:
                    lblGPSCheck.Text = "Đã kết nối GPS";
                    break;
                case false:
                    lblGPSCheck.Text = "Vui lòng kết nối GPS để sử dụng ứng dụng";
                    break;
            }*/
            switch (netcheck)
            {
                case true:
                    lblNetworkCheck.Text = "Đã kết nối mạng";
                    break;
                case false:
                    lblNetworkCheck.Text = "Vui lòng kết nối mạng để sử dụng ứng dụng";
                    break;
            }
            if ((netcheck == true))
            {
                switchstate = true;
                btnSwitch.IsVisible = true;
                btnSwitch.Text = "Bấm vào đây để tiếp tục";
            }
        }
    }
}