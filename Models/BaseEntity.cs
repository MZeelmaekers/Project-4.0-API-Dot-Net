using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Models
{
    public class BaseEntity
    {
        public DateTime CreatedAt {get;set;}

        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow.AddHours(1);
        }
}
}
