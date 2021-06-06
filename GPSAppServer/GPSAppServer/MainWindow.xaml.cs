using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR;
using System.ComponentModel;
using System.Net;
using System.Collections.ObjectModel;
using Microsoft.Maps.MapControl.WPF;

namespace GPSAppServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] clientlist = new string[100];
        double[] latitudearr = new double[100];
        double[] longitudearr = new double[100];
        int clientcount;
        int foundno;
        IHubProxy Proxy;
        ObservableCollection<Client> clientcollection = new ObservableCollection<Client>();
        public MainWindow()
        {
            InitializeComponent();
            clientcount = 0;
            MainBoard.ItemsSource = clientcollection;
        }

        public void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.Connect();
            Location mylocation = new Location(10.759223, 106.666033);
            MyMap.SetView(mylocation, 30, 0);
            MainBoard.Columns[0].Header = "ID";
            MainBoard.Columns[1].Header = "Tên đăng nhập";
            MainBoard.Columns[2].Header = "Vĩ độ";
            MainBoard.Columns[3].Header = "Kinh độ";
        }

        public void Align(DataGridColumn c, TextAlignment a)
        {
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, a));
            c.CellStyle = s;
        }

        public void Connect()
        {
            HubConnection hub = new HubConnection("http://gpsapp.somee.com/signalr/");
            hub.TraceLevel = TraceLevels.All;
            hub.TraceWriter = Console.Out;
            this.Proxy = hub.CreateHubProxy("HubCenter");
            this.Proxy.On("UpdateTest", () => this.UpdateTest());
            this.Proxy.On<string, double, double>("UpdateClient", (clientname, latitude, longitude) => this.Dispatcher.Invoke(() => this.UpdateClient(clientname, latitude, longitude)));
            this.Proxy.On<string>("UpdateLogOut", (username) => this.Dispatcher.Invoke(() => this.UpdateLogOut(username)));
            try
            {
                hub.Start().Wait();
                MessageBox.Show("Connected to server");
            }
            catch
            {
                MessageBox.Show("Can't connect to server");
            }
            try
            {
                this.Proxy.Invoke("StartReceive");
            }
            catch
            { }
        }

        public void UpdateTable()
        {
            MainBoard.ItemsSource = null;
            MainBoard.Items.Refresh();
            clientcollection.Clear();
            for (int i = 1; i <= clientcount; i++)
            {
                clientcollection.Add(new Client() { ID = i, Username = clientlist[i], Latitude = latitudearr[i], Longitude = longitudearr[i] });
            }
            MainBoard.ItemsSource = clientcollection;
            MainBoard.Items.Refresh();
            MainBoard.Columns[0].Header = "ID";
            MainBoard.Columns[1].Header = "Tên đăng nhập";
            MainBoard.Columns[2].Header = "Vĩ độ";
            MainBoard.Columns[3].Header = "Kinh độ";
        }

        public class Client
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public void UpdateClient(string clientName, double latitude, double longitude)
        {
            bool isFound = false;
            for (int i = 0; i <= clientcount; i++)
            {
                if (clientName == clientlist[i])
                {
                    latitudearr[i] = latitude;
                    longitudearr[i] = longitude;
                    isFound = true;
                }
            }
            if (isFound == false)
            {
                clientcount += 1;
                clientlist[clientcount] = clientName;
                latitudearr[clientcount] = latitude;
                longitudearr[clientcount] = longitude;
            }
            UpdateMap();
            UpdateTable();
        }

        public void UpdateLogOut(string username)
        {
            for (int i = 0; i <= clientcount; i++)
            {
                if (username == clientlist[i])
                {
                    foundno = i;
                    break;
                }
            }
            if (foundno != clientcount)
            {
                for (int j = foundno + 1; j <= clientcount; j++)
                {
                    clientlist[j - 1] = clientlist[j];
                    latitudearr[j - 1] = latitudearr[j];
                    longitudearr[j - 1] = longitudearr[j];
                }
                clientcount -= 1;
            }
            if (foundno == clientcount)
            {
                clientlist[foundno] = "";
                latitudearr[foundno] = 0;
                longitudearr[foundno] = 0;
                clientcount -= 1;
            }
            UpdateMap();
            UpdateTable();
        }

        private void UpdateMap()
        {
            MyMap.Children.Clear();
            if (clientcount > 0)
            {
                for (int i = 1; i <= clientcount; i++)
                {
                    CreateNewPushPin(i);
                }
                Location viewlc = new Location();
                viewlc.Longitude = longitudearr[clientcount];
                viewlc.Latitude = latitudearr[clientcount];
                MyMap.SetView(viewlc, 18, 0);
            }
        }

        private void CreateNewPushPin(int i)
        {
            Location lc = new Location();
            lc.Longitude = longitudearr[i];
            lc.Latitude = latitudearr[i];
            Pushpin pin = new Pushpin();
            pin.Location = lc;
            pin.Content = i.ToString();
            MyMap.Children.Add(pin);
            MyMap.SetView(lc, 30, 0);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.Proxy.Invoke("StopReceive");
        }

        public void UpdateTest()
        {
            MessageBox.Show("Test");
        }

        public void ErrorHandling(Action action)
        {
            try { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Client");
                base.Close();
            }
        }

    }
}
