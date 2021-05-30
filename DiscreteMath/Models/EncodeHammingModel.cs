using DiscreteMath.Models.EncodingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class EncodeHammingModel : ExerciseModel
    {
        public EncodeHammingData InputData { get; set; }

        public string EncodedMessage { get; set; }

        public EncodeHammingModel() { }

        public EncodeHammingModel(EncodeHammingData data)
        {
            InputData = data;
        }
    }
}