using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class CheckCodeUnambiguityModel : ExerciseModel
    {
        public CheckCodeUnambiguityData InputData { get; set; }

        public int? SelectedAnswer { get; set; }

        public CheckCodeUnambiguityModel() { }

        public CheckCodeUnambiguityModel(CheckCodeUnambiguityData data)
        {
            InputData = data;
        }
    }
}