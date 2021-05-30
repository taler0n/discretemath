using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class CheckCodeOptimalityModel : ExerciseModel
    {
        public CheckCodeOptimalityData InputData { get; set; }

        public int? SelectedAnswer { get; set; }

        public CheckCodeOptimalityModel() { }

        public CheckCodeOptimalityModel(CheckCodeOptimalityData data)
        {
            InputData = data;
        }
    }
}