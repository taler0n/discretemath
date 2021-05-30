using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DiscreteMath.Models
{
    public class ExerciseContext: DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
    }
}