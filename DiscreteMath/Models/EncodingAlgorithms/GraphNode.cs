using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class GraphNode
    {
        public Object NodeContent { get; set; }
        public List<GraphNode> Connections { get; set; }

        public GraphNode(Object content)
        {
            NodeContent = content;
            Connections = new List<GraphNode>();
        }

        public GraphNode(Object content, List<GraphNode> connections)
        {
            NodeContent = content;
            Connections = connections;
        }
    }
}