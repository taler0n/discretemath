using System.Collections.Generic;

namespace DiscreteMath.Models.EncodingData
{
    public class CheckUnambiguousCodeExistanceData
    {
        public int AlphabetLength { get; set; }
        public List<int> CodewordLengths { get; set; }
    }
}