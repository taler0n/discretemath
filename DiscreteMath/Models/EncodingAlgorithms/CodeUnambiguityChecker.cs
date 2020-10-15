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
                for (int i = 0; i < codeword.Length; i++)
                {
                    string prefix = codeword.Substring(0, i);
                    string tail = codeword.Substring(i);
                    List<string> postfixes = new List<string>();
                    Queue<string> tails = new Queue<string>();
                    tails.Enqueue(tail);
                    while (tails.Count > 0)
                    {
                        string currentTail = tails.Dequeue();
                        foreach (var middleWord in code.ElementaryCodes.Values)
                        {
                            if (middleWord.Length <= currentTail.Length && codeword != middleWord 
                                && currentTail.Substring(0, middleWord.Length) == middleWord)
                            {
                                string newTail = currentTail.Substring(middleWord.Length);
                                tails.Enqueue(newTail);
                                postfixes.Add(newTail);
                            }
                        }
                        
                    }

                    if (postfixes.Count == 0)
                    {
                        continue;
                    }

                    GraphNode prefixNode = graph.GetNode(prefix);
                    if (prefixNode == null)
                    {
                        prefixNode = new GraphNode(prefix);
                        graph.AddNode(prefixNode);
                    }
                    foreach (var postfix in postfixes)
                    {
                        if (postfix != codeword)
                        {
                            GraphNode postfixNode = graph.GetNode(postfix);
                            if (postfixNode == null)
                            {
                                postfixNode = new GraphNode(postfix);
                                graph.AddNode(postfixNode);
                            }
                            prefixNode.Connections.Add(postfixNode);
                        }
                    }
                }
            }
            return !graph.CycleWithNodeExists(startNode);
        }
    }
}