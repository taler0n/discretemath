using System;
using System.Collections.Generic;

namespace DiscreteMath.Models
{
    public class OrientedGraph
    {
        private Dictionary<Object, GraphNode> _nodes;

        public OrientedGraph() 
        {
            _nodes = new Dictionary<object, GraphNode>();
        }

        public void AddNode(GraphNode node)
        {
            _nodes.Add(node.NodeContent, node);
        }

        public void RemoveNode(Object nodeContent)
        {
            _nodes.Remove(nodeContent);
        }

        public bool ContainsNode(Object nodeContent)
        {
            return _nodes.ContainsKey(nodeContent);
        }

        public GraphNode GetNode(Object nodeContent)
        {
            if (_nodes.ContainsKey(nodeContent))
            {
                return _nodes[nodeContent];
            }
            else
            {
                return null;
            }
        }

        public bool CycleWithNodeExists(GraphNode startNode)
        {
            if (_nodes.ContainsKey(startNode.NodeContent))
            {
                Dictionary<Object, int> marks = new Dictionary<object, int>();
                foreach (var node in _nodes.Values)
                {
                    marks.Add(node.NodeContent, 0);
                }
                marks[startNode.NodeContent] = 2;
                Stack<GraphNode> deepSearchStack = new Stack<GraphNode>();
                deepSearchStack.Push(startNode);

                while (deepSearchStack.Count > 0)
                {
                    GraphNode node = deepSearchStack.Pop();
                    if (marks[node.NodeContent] == 2)
                    {
                        return true;
                    }
                    if (marks[node.NodeContent] == 0)
                    {
                        marks[node.NodeContent] = 1;
                        foreach (var neighbor in node.Connections)
                        {
                            deepSearchStack.Push(neighbor);
                        }
                    }
                }
                return false;
            }

            return false;
        }
    }
}