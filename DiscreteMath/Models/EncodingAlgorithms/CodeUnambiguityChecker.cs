using System;
using System.Collections.Generic;

namespace DiscreteMath.Models.EncodingAlgorithms
{
    public static class CodeUnambiguityChecker
    {
        public static bool CheckMcmillanInequality(List<int> codewordLengths, int alphabetLength)
        {
            double sum = 0;
            foreach (var wordLength in codewordLengths)
            {
                sum += 1 / Math.Pow(alphabetLength, wordLength);
            }
            if (sum <= 1)
            {
                return true;
            }
            return false;
        }

        public static bool CheckPrefixProperty(Code code)
        {
            foreach (var code1 in code.ElementaryCodes)
            {
                foreach (var code2 in code.ElementaryCodes)
                {
                    if (code1.Key != code2.Key && code1.Value.Length <= code2.Value.Length)
                    {
                        string prefix = code2.Value.Substring(0, code1.Value.Length);
                        if (prefix == code1.Value)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool CheckMarkovTheorem(Code code)
        {
            OrientedGraph graph = new OrientedGraph();
            GraphNode startNode = new GraphNode(String.Empty);
            graph.AddNode(startNode);
            foreach (var codeword in code.ElementaryCodes.Values)
            {
                for (int i = 1; i < codeword.Length; i++)
                {
                    string prefix = codeword.Substring(0, i);
                    List<string> postfixes = GetPostfixes(codeword.Substring(i), code);
                    GraphNode prefixNode = graph.GetNode(prefix);
                    if (prefixNode == null)
                    {
                        prefixNode = new GraphNode(prefix);
                        graph.AddNode(prefixNode);
                    }
                    foreach (var postfix in postfixes)
                    {
                        GraphNode postfixNode = graph.GetNode(postfix);
                        if (postfixNode == null)
                        {
                            postfixNode = new GraphNode(prefix);
                            graph.AddNode(postfixNode);
                        }
                        prefixNode.Connections.Add(postfixNode);
                    }
                }
            }
            return graph.CycleWithNodeExists(startNode);
        }

        private static List<string> GetPostfixes(string tail, Code code)
        {
            List<string> postfixes = new List<string>();
            foreach (var codeword in code.ElementaryCodes.Values)
            {
                if (codeword.Length <= tail.Length && tail.Substring(codeword.Length) == codeword)
                {
                    List<string> newPostfixes = GetPostfixes(tail.Substring(codeword.Length), code);
                    postfixes.AddRange(newPostfixes);
                }
            }
            if (postfixes.Count == 0)
            {
                postfixes.Add(tail);
            }
            return postfixes;
        }
    }
}