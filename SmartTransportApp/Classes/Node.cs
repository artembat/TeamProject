using System;
namespace SmartTransportApp.Classes
{
    public class Node
    {
        public bool IsTransfer { get; set; }
        public string CodeFrom { get; set; }
        public string CodeTo { get; set; }
        public string TitleFrom { get; set; }
        public string TitleTo { get; set; }
        public string TransportTypeFrom { get; set; }
        public string TransportTypeTo { get; set; }
        public string StationTypeFrom { get; set; }
        public string StationTypeTo { get; set; }
        public string TrainTitle { get; set; }
    }
}
