using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AspNet.SignalR.Client;
using static GPSAppClient.ConnectClass;

namespace GPSAppClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public bool isConnected, isLoaded;
        public string mainusername;
        ConnectClass connection;
        IHubProxy Proxy;
        public StartPage parent;
        public MainPage mp;
        public bool statebol = false;

        public LoginPage()
        {
            InitializeComponent();
            LogIn.Clicked += LogIn_Clicked;
            connection = new ConnectClass();
            if (connection.isLoaded == false)
            {
                CallConnect();
            }
        }

        public async void CallConnect()
        {
            await Connect();
        }

        public async Task Connect()
        {
            HubConnection hub = new HubConnection("http://gpsapp.somee.com/signalr/");
            hub.TraceLevel = TraceLevels.All;
            hub.TraceWriter = Console.Out;
            connection.isLoaded = true;
            this.Proxy = hub.CreateHubProxy("HubCenter");
            this.Proxy.On("LogOutConfirmed", () => this.ExecuteLogOut());
            try
            {
                await hub.Start();
                connection.isConnected = true;
                await DisplayAlert("Thông báo", "Đã kết nối đến server", "OK");
                Username.IsEnabled = true;
            }
            catch
            {
                connection.isConnected = false;
                await DisplayAlert("Lỗi", "Không thể kết nối đến server. Bạn sẽ sử dụng ứng dụng dưới chế độ offline", "OK");
                await App.MyNavigationPage.PushAsync(mp, true);
            }
            return;
        }

        private void LogIn_Clicked(object sender, EventArgs e)
        {
            if (Username.Text != null)
            {
                mainusername = Username.Text.ToString();
            }
            else
            { mainusername = string.Empty; }
            mp = new MainPage(this);
            mp.name = mainusername;
            App.MyNavigationPage.PushAsync(mp, true);
            }

        public void ServerError()
        {
            DisplayAlert("Lỗi", "Máy chủ hiện không nhận dữ liệu", "OK");
        }

        public async Task Update(string mainusername, double latitude, double longitude)
        {
            await Proxy.Invoke("Update", mainusername, latitude, longitude);
        }

        public async Task LogOut()
        {
            await Proxy.Invoke("LogOut", mainusername);
            connection.isConnected = false;
        }

        public void ExecuteLogOut()
        {            
            HubConnection hub = new HubConnection("http://gpsapp.somee.com/signalr/");
            hub.Stop();
        }
    }
}