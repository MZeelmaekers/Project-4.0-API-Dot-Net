using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Prediction { get; set; }
        public double Accuracy { get; set; }

        public ICollection<Plant> Plant { get; set; }
    }
}
