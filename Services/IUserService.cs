using Project40_API_Dot_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
    }
}
