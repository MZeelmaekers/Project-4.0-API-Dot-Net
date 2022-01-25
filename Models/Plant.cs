using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class Plant: BaseEntity
    {
        public int Id { get; set; }
        public string FotoPath { get; set; }
        public string? Location { get; set; }


        public int? UserId { get; set; }
        public User User { get; set; }
        public int? ResultId { get; set; }
        public Result Result { get; set; }
    }
}
