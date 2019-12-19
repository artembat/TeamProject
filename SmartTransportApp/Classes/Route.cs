using System.Collections.Generic;

namespace SmartTransportApp.Classes
{
    public class Route
    {
        public List<Node> Nodes { get; set; }

        public void AddNode( Node node)
        {
            Nodes.Add(node);
        }

        public Route()
        {
            Nodes = new List<Node>();
        }
    }
}
