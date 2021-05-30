using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public abstract class ExerciseModel
    {
        public class Answer
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public Answer(int id, string text)
            {
                Id = id;
                Text = text;
            }
        }

        public static Answer[] YesNoAnswers = new Answer[] { new Answer(0, "Является"), new Answer(1, "Не является") };

        public string Comment { get; set; }

    }
}