using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class GetCorrectingCodeParametersModel : ExerciseModel
    {
        public GetCorrectingCodeParametersData InputData { get; set; }

        public int? CodeDistance { get; set; }
        public int? DetectableErrors { get; set; }
        public int? FixableErrors { get; set; }

        public GetCorrectingCodeParametersModel() { }

        public GetCorrectingCodeParametersModel(GetCorrectingCodeParametersData data)
        {
            InputData = data;
        }
    }
}