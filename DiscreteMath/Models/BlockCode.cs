using System;
using System.Collections.Generic;

namespace DiscreteMath.Models
{
    public class BlockCode : Code
    {
        public int BlockSize { get; private set; }

        public BlockCode(int blockSize) : base()
        {
            BlockSize = blockSize;
        }

        public BlockCode(Dictionary<char, string> elementaryCodes, int blockSize) : base(elementaryCodes)
        {
            BlockSize = blockSize;
            foreach (var code in ElementaryCodes.Values)
            {
                if (code.Length != BlockSize)
                {
                    throw new ArgumentException();
                }
            }
        }

        public override void Set(char symbol, string code)
        {
            if (code.Length != BlockSize)
            {
                throw new ArgumentException();
            }
            base.Set(symbol, code);
        }

        public int GetCodeDistance()
        {
            if (ElementaryCodes.Count < 2)
            {
                return 0;
            }
            int codeDistance = -1;
            foreach (var codePair1 in ElementaryCodes)
            {
                foreach (var codePair2 in ElementaryCodes)
                {
                    if (!codePair1.Key.Equals(codePair2.Key))
                    {
                        int distance = GetHammingDistance(codePair1.Value, codePair2.Value);
                        if (codeDistance == -1 || distance < codeDistance)
                        {
                            codeDistance = distance;
                        }
                    }
                }
            }
            return codeDistance;
        }

        private int GetHammingDistance(string s1, string s2)
        {
            int count = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] != s2[i])
                {
                    count++;
                }
            }
            return count;
        }

        public int GetDetectableErrors(int codeDistance)
        {
            if (codeDistance < 1)
            {
                throw new ArgumentException();
            }
            return codeDistance - 1;
        }

        public int GetFixableErrors(int codeDistance)
        {
            if (codeDistance < 1)
            {
                throw new ArgumentException();
            }
            return (codeDistance - 1) / 2;
        }
    }
}