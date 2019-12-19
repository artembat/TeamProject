using System;
using System.Collections.Generic;
using System.Linq;
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
using SmartTransportApp.Classes;

namespace SmartTransportApp
{
    
    public partial class MainWindow : Window
    {
        public const string api_key = "2bab6c48-70fa-46a8-9383-bf5fa3c132af";
        public TransportManager Manager;
        public List<List<string>> Stations;
        public MainWindow()
        {
            InitializeComponent();
            Manager = new TransportManager(api_key);
            Stations = Manager.GetAllStations();
        }

        private void RevertBtn_Click(object sender, RoutedEventArgs e)
        {
            var tempText = FromCity.Text;
            FromCity.Text = ToCity.Text;
            ToCity.Text = tempText;
        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoutesView.Clear();
                var CityA = Manager.FindStation(FromCity.Text, Stations);
                var CityB = Manager.FindStation(ToCity.Text, Stations);
                var CityACode = CityA[0][4];
                var CityBCode = CityB[0][4];
                var TripDate = Date.Text;
                var Routes = Manager.GetRoutes(CityACode, CityBCode, TripDate);
                var RouteNumber = 1;

                foreach (Route route in Routes)
                {
                    RoutesView.Text += "Route #" + Convert.ToString(RouteNumber) + "\n";
                    foreach (Node node in route.Nodes)
                    {
                        if (node.IsTransfer)
                        {
                            RoutesView.Text += "Transfer from: " + node.TitleFrom + " to: " + node.TitleTo + "\n";
                        }
                        else
                        {
                            RoutesView.Text += "From: " + node.TitleFrom + " to: " + node.TitleTo + " " + node.TransportTypeFrom + "\n";
                        }
                    }
                    RouteNumber++;
                }
            }
            catch (Exception)
            {
                RoutesView.Text = "Error";
            }
            
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> results;

            if (AirportsOnly.IsChecked == true)
            {
                var stations = Manager.FindStation(Search.Text, Stations);
                results = Manager.FindAirport(stations);
            }
            else
            {
                results = Manager.FindStation(Search.Text, Stations);
            }
            
            foreach (var result in results)
            {
                StationsBox.Text += result[3] + ", " + result[2] + ", " + result[1] + ", " + result[0] + "\n";
            }
        }
    }
}
