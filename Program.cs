using System;
using System.Collections.Generic;
using System.Text;
using SmartTransportApp.Classes;

namespace SmartTransportApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            const string api_key = "2bab6c48-70fa-46a8-9383-bf5fa3c132af";
            var manager = new TransportManager(api_key);
            var stations = manager.GetAllStations();
            var search = manager.FindStation("Москва", stations);
            var search2 = manager.FindStation("Санкт-Петербург", stations);

            /*  foreach ( var item in search )
              {
                  foreach ( var subitem in item )
                  {
                      Console.Write("[" + subitem + "] ");
                  }

                  Console.WriteLine();
              } */

            List<Route> routes = manager.GetRoutes("s2000001", "s9602494", "2019-12-19");

            foreach (Route route in routes)
            {
                foreach (Node n in route.Nodes)
                {
                    Console.WriteLine("Следуйте от {0}: {1} на {2} к {3}: {4} на {5}: {6}",
                        n.StationTypeFrom, n.TitleFrom, n.TransportTypeFrom,
                        n.StationTypeTo, n.TitleTo, n.TransportTypeTo, n.TrainTitle);
                }

                Console.WriteLine("\n");

            }

            
            
        /*    foreach ( Route r in routes )
            {
                foreach (var node in r.Nodes)
                {
                    Console.WriteLine(node.TitleFrom);
                    Console.WriteLine(node.TitleTo);
                    Console.WriteLine(node.CodeFrom);
                    Console.WriteLine(node.CodeTo);
                    Console.WriteLine(node.IsTransfer);
                    Console.WriteLine(node.StationTypeFrom);
                    Console.WriteLine(node.StationTypeTo);
                    Console.WriteLine(node.TransportTypeFrom);
                    Console.WriteLine(node.TransportTypeTo);
                }
                Console.WriteLine();
            } */
            
        }
    } 
}
