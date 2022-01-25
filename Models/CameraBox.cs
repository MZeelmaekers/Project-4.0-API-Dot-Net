using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class CameraBox: BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string QrCode { get; set; }
        public int? UserId { get; set; }

        public User User { get; set; }
    }
}
