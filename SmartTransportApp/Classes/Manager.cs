using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace SmartTransportApp.Classes
{
    public class TransportManager
    {
        private string token;
        public List<string> Stations { get; set; }

        public void SetToken(string tokenToSet)
        {
            token = tokenToSet;
        }

        private string GETRequest(string url)
        {
            HttpWebRequest request =
            (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "Mozilla/5.0 ....";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("HTTP response received");
            StreamReader reader = new StreamReader(response.GetResponseStream());
            //StringBuilder output = new StringBuilder();
            Console.WriteLine("Reading content");
            string data = reader.ReadToEnd();
            //byte[] data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            Console.WriteLine("Finished reading content");
            //output.Append(Encoding.UTF8.GetString(data));
            response.Close();
            // stations = output.ToString();
            return data; //stations;
        }

        public static bool HasProperty(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }

        public List<List<string>> GetAllStations()
        {
            string query = string.Format("https://api.rasp.yandex.net/v3.0/stations_list/?apikey={0}&lang=ru_RU&format=json", token);
            Console.WriteLine(query);
            dynamic response = JsonConvert.DeserializeObject(GETRequest(query));
            List<List<string>> search = new List<List<string>>();

            foreach (var country in response.countries)
            {
                foreach (var region in country.regions)
                {
                    foreach (var settlement in region.settlements)
                    {
                        foreach (var station in settlement.stations)
                        {
                            var item = new List<string>
                            {
                                country.title.ToString(),
                                region.title.ToString(),
                                settlement.title.ToString(),
                                station.title.ToString(),
                                station.codes.yandex_code.ToString(),
                                station.station_type.ToString(),
                                station.transport_type.ToString()
                            };

                            search.Add(item);
                        }
                    }
                }
            }

            return search;
        }

        public List<Route> GetRoutes(string pointACode, string pointBCode, string date)
        {
            string query = string.Format("https://api.rasp.yandex.net/v3.0/search/?apikey={0}&format=json&from={1}&to={2}&lang=ru_RU&page=1&date={3}&transfers=true", token, pointACode, pointBCode, date);
            Console.WriteLine(query);
            dynamic response = JsonConvert.DeserializeObject(GETRequest(query));
           // Console.WriteLine(response);
            List<Route> routes = new List<Route>();
            foreach ( var segment in response.segments)
            {
                Console.WriteLine(segment);

                Route route = new Route();

                try
                {
                    var details = segment.details;
                    // if has details
                    foreach (var d in details)
                    {
                        //Console.WriteLine(d.thread.title);
                        
                        try
                        {
                            // if does not have transfers
                            Node n = new Node();
                            n.CodeFrom = d.from.code;
                            n.CodeTo = d.to.code;
                            n.IsTransfer = false;
                            n.StationTypeFrom = d.from.station_type_name;
                            n.StationTypeTo = d.to.station_type_name;
                            n.TitleFrom = d.from.title;
                            n.TitleTo = d.to.title;
                            n.TransportTypeFrom = d.from.transport_type;
                            n.TransportTypeTo = d.to.transport_type;
                            route.AddNode(n);
                            Console.WriteLine("OK");
                        }
                        catch (Exception)
                        {
                            // if has transfers
                            Node n = new Node();
                            n.CodeFrom = d.transfer_from.code;
                            n.CodeTo = d.transfer_to.code;
                            n.IsTransfer = d.is_transfer;
                            n.StationTypeFrom = d.transfer_from.station_type_name;
                            n.StationTypeTo = d.transfer_to.station_type_name;
                            n.TitleFrom = d.transfer_from.title;
                            n.TitleTo = d.transfer_to.title;
                            n.TransportTypeFrom = d.transfer_from.transport_type;
                            n.TransportTypeTo = d.transfer_to.transport_type; 
                            //n.TrainTitle = d.thread.title;
                            route.AddNode(n);
                            Console.WriteLine("NOT OK");
                        }

                    }
                }
                catch(Exception e)
                {
                    // if does not have details
                    try
                    {
                        Node n = new Node();
                        // if does not have transfers
                        n.CodeFrom = segment.from.code;
                        n.CodeTo = segment.to.code;
                        n.IsTransfer = false;
                        n.StationTypeFrom = segment.from.station_type_name;
                        n.StationTypeTo = segment.to.station_type_name;
                        n.TitleFrom = segment.from.title;
                        n.TitleTo = segment.to.title;
                        n.TransportTypeFrom = segment.from.transport_type;
                        n.TransportTypeTo = segment.to.transport_type;
                       // n.TrainTitle = segment.thread.title;
                        route.AddNode(n);
                        Console.WriteLine("OK");
                    }
                    catch (Exception)
                    {
                        Node n = new Node();
                       // Console.WriteLine("Error");
                        // if has transfers
                        n.CodeFrom = segment.transfer_from.code;
                        n.CodeTo = segment.transfer_to.code;
                        n.IsTransfer = false;
                        n.StationTypeFrom = segment.transfer_from.station_type_name;
                        n.StationTypeTo = segment.transfer_to.station_type_name;
                        n.TitleFrom = segment.transfer_from.title;
                        n.TitleTo = segment.transfer_to.title;
                        n.TransportTypeFrom = segment.transfer_from.transport_type;
                        n.TransportTypeTo = segment.transfer_to.transport_type;
                      //  n.TrainTitle = segment.thread.title;
                        route.AddNode(n);
                        Console.WriteLine("NOT OK");
                    }

                }

                routes.Add(route);
            }

            return routes;
            
        }

        public List<List<string>> FindStation(string station_name, List<List<string>> stations)
        {
            var search = new List<List<string>>();

            foreach ( var station in stations)
            {
                if ( station.Contains(station_name) )
                {
                    search.Add(station);
                }
            }

            return search;
            
        }

        public List<List<string>> FindAirport(List<List<string>> stations)
        {
            var search = new List<List<string>>();

            foreach (var station in stations)
            {
                if (station[5] == "airport")
                {
                    search.Add(station);
                }
            }

            return search;
        }

        public TransportManager(string auth_token)
        {
            SetToken(auth_token);
            Stations = new List<string>();
        }
    }
}
