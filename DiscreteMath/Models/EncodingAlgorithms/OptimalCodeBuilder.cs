using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models.EncodingAlgorithms
{
    public static class OptimalCodeBuilder
    {
        private class NodeContent
        {
            public char Symbol { get; set; }
            public double Frequency { get; set; }
            public string Code { get; set; }
            public NodeContent(char symbol = (char)0, double frequency = 0, string code = "")
            {
                Symbol = symbol;
                Frequency = frequency;
                Code = code;
            }
            public int CompareTo(NodeContent other)
            {
                return Frequency.CompareTo(other.Frequency);
            }
        }
        public static FrequencyCode GetOptimalCode(string alphabet, Dictionary<char, double> frequencyList)
        {
            int q = frequencyList.Count % (alphabet.Length - 1);
            if (q == 0)
            {
                q = alphabet.Length - 1;
            }
            if (q == 1)
            {
                q = alphabet.Length;
            }
            List<NodeContent> looseVertices = new List<NodeContent>();
            OrientedGraph graph = new OrientedGraph();
            foreach (var item in frequencyList)
            {
                NodeContent content = new NodeContent(item.Key, item.Value);
                AddToListWithSort(looseVertices, content);
                graph.AddNode(new GraphNode(content));
            }

            while (looseVertices.Count > 1)
            {
                NodeContent newContent = new NodeContent();
                GraphNode newNode = new GraphNode(newContent);
                for (int i = 0; i < q; i++)
                {
                    NodeContent content = looseVertices[0];
                    newContent.Frequency += content.Frequency;
                    GraphNode node = graph.GetNode(content);
                    newNode.Connections.Add(node);
                    looseVertices.RemoveAt(0);
                }
                AddToListWithSort(looseVertices, newContent);
                graph.AddNode(newNode);
                q = alphabet.Length;
            }
            FrequencyCode code = new FrequencyCode();
            Queue<GraphNode> broadSearchQueue = new Queue<GraphNode>();
            broadSearchQueue.Enqueue(graph.GetNode(looseVertices[0]));

            while (broadSearchQueue.Count > 0)
            {
                GraphNode node = broadSearchQueue.Dequeue();
                NodeContent content = (NodeContent)node.NodeContent;
                if (node.Connections.Count == 0)
                {
                    code.Set(content.Symbol, content.Code, content.Frequency);
                }
                else
                {
                    for (int i = 0; i < node.Connections.Count; i++)
                    {
                        NodeContent childContent = (NodeContent)node.Connections[i].NodeContent;
                        childContent.Code = content.Code + alphabet[i];
                        broadSearchQueue.Enqueue(node.Connections[i]);
                    }
                }
            }
            return code;
        }
        private static void AddToListWithSort(List<NodeContent> list, NodeContent newNode)
        {
            bool greatest = true;
            for (int i = 0; i < list.Count; i++)
                if (list[i].CompareTo(newNode) >= 0)
                {
                    list.Insert(i, newNode);
                    greatest = false;
                    break;
                }
            if (greatest)
                list.Add(newNode);
        }
    }
}