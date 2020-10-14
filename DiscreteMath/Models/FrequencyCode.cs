using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscreteMath.Models
{
    public class FrequencyCode : Code
    {
        public Dictionary<char, double> CodeFrequencies { get; private set; }

        public FrequencyCode() : base()
        {
            CodeFrequencies = new Dictionary<char, double>();
        }

        public FrequencyCode(Dictionary<char, string> elementaryCodes, Dictionary<char, double> codeFrequencies) : base(elementaryCodes)
        {
            if (elementaryCodes.Keys.Count != codeFrequencies.Keys.Count || !elementaryCodes.Keys.SequenceEqual(codeFrequencies.Keys))
            {
                throw new ArgumentException();
            }
            CodeFrequencies = codeFrequencies;
        }

        public void Set(char symbol, string code, double frequency)
        {
            Set(symbol, code);
            CodeFrequencies.Add(symbol, frequency);
        }

        public double GetCodeOptimality()
        {
            double sum = 0;
            foreach (var key in ElementaryCodes.Keys)
            {
                sum += ElementaryCodes[key].Length * CodeFrequencies[key];
            }
            return sum;
        }
    }
}