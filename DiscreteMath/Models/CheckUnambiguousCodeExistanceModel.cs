using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class CheckUnambiguousCodeExistanceModel : ExerciseModel
    {
        public CheckUnambiguousCodeExistanceData InputData { get; set; }

        public int? SelectedAnswer { get; set; }

        public CheckUnambiguousCodeExistanceModel() { }

        public CheckUnambiguousCodeExistanceModel(CheckUnambiguousCodeExistanceData data)
        {
            InputData = data;
        }
    }
}