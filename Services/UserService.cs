using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project40_API_Dot_NET.Data;
using Project40_API_Dot_NET.Helpers;
using Project40_API_Dot_NET.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project40_API_Dot_NET.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly PlantContext _plantContext;
        public UserService(IOptions<AppSettings> appSettings, PlantContext plantContext)
        {
            _appSettings = appSettings.Value;
            _plantContext = plantContext;
        }
        public User Authenticate(string email, string password)
        {
            var user = _plantContext.Users.SingleOrDefault(x => x.Email == email);
            // return null if user not found
            if (user == null)
                return null;
            // Check if the hashes of the passwords match
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserID", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Name", user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            user.Password = null;
            return user;
        }
    }
}
