using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class BuildOptimalCodeModel : ExerciseModel
    {
        public BuildOptimalCodeData InputData { get; set; }

        public Dictionary<char, string> ElementaryCodes { get; set; }

        public BuildOptimalCodeModel() { }

        public BuildOptimalCodeModel(BuildOptimalCodeData data)
        {
            InputData = data;
        }
    }
}