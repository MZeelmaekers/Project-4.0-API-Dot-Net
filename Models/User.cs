using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public string? Hometown { get; set; }

        public ICollection<CameraBox> CameraBoxes { get; set; }
        public ICollection<Plant> Plants { get; set; }
    }
}
