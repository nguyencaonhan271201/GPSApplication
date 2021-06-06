using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR.Client;
using System.ComponentModel;

namespace GPSAppClient
{
	public partial class MainPage : ContentPage
	{
        public double currentlong, currentlat;
        Geocoder geocoder;
        ConnectClass connect;
        LoginPage lg;
        public string name;
        bool set;
        BackgroundWorker GetGPS = new BackgroundWorker();
        public MainPage(LoginPage parent)
		{
            InitializeComponent();
            btnGetLocation.Clicked += BtnGetLocation_Clicked;
            ChooseMapType.SelectedIndexChanged += ChooseMapType_SelectedIndexChanged;
            ChooseMapType.SelectedIndex = 2;
            set = false;
            btnClose.Clicked += BtnClose_Clicked;
            connect = new ConnectClass();
            lg = parent;
            base.OnAppearing();
            GetGPS.DoWork += GetGPS_DoWork;
            GetGPS.RunWorkerCompleted += GetGPS_RunWorkerCompleted;          
        }

        private void GetGPS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null) { }
            else DisplayAlert("Lỗi", e.Error.ToString(), "OK");
        }

        private void GetGPS_DoWork(object sender, DoWorkEventArgs e)
        {
            var t = btnGetLocation as IButtonController;
            t.SendClicked();
            Update();
        }

        protected override async void OnAppearing()
        {
            try
            {
                FirstCallConnect();
            }
            catch
            {
                await DisplayAlert("Lỗi", "Không thể định vị địa điểm", "OK");
                FirstCallConnect();
            }
        }

        public void FirstCallConnect()
        {
            var t = btnGetLocation as IButtonController;
            t.SendClicked();
            Update();
        }

        private async void BtnClose_Clicked(object sender, EventArgs e)
        {
            await CrossGeolocator.Current.StopListeningAsync();
            await lg.LogOut();
            await DisplayAlert("", "Thank you for using!", "OK");
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());     
        }

        private async void Update()
        {
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(2), 0.1, false, null);
            CrossGeolocator.Current.PositionChanged += Current_PositionChangedAsync;
        }

        private async void Current_PositionChangedAsync(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            await RetrieveLocation();
        }

        private void ChooseMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ChooseMapType.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    MyMap.MapType = MapType.Hybrid;
                    break;
                case 1:
                    MyMap.MapType = MapType.Satellite;
                    break;
                case 2:
                    MyMap.MapType = MapType.Street;
                    break;
            }
        }

        private async void BtnGetLocation_Clicked(object sender, EventArgs e)
        {
            await RetrieveLocation();
        }

        private async Task RetrieveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;

            var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
            txtLat.Text = "Vĩ độ hiện tại (Current latitude): " + position.Latitude.ToString(".000");
            txtLong.Text = "Kinh độ hiện tại (Current longitude): " + position.Longitude.ToString(".000");
            switch (set)
            {
                case false:
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude)
                             , Distance.FromMeters(30)));
                    set = true;
                    break;
                case true:
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude)
                             , MyMap.VisibleRegion.Radius));
                    break;
            }
            

            geocoder = new Geocoder();
            var pos = new Position (position.Latitude, position.Longitude);
            var possibleaddresses = await geocoder.GetAddressesForPositionAsync(pos);
            txtAdd.Text = "Địa chỉ: " + possibleaddresses.FirstOrDefault();

            try
            {
                await lg.Update(name, position.Latitude, position.Longitude);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", ex.Message.ToString(), "OK");
            }
        }

    }
}
