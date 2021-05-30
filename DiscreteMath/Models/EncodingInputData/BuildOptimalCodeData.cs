using System.Collections.Generic;

namespace DiscreteMath.Models.EncodingData
{
    public class BuildOptimalCodeData
    {
        public string CodeAlphabet { get; set; }
        public Dictionary<char, double> FrequencyList { get; set; }

    }
}