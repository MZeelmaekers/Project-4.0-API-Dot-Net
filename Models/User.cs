using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class User: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role{ get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
        public string? Hometown { get; set; }
        public int? SuperVisorId { get; set; }
        [NotMapped]
        public string Token { get; set; }

        public User SuperVisor { get; set; }
        public ICollection<CameraBox> CameraBoxes { get; set; }
        public ICollection<Plant> Plants { get; set; }
        public ICollection<User> Workers { get; set; }
    }
}
