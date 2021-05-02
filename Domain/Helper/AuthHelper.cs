using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Model.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Helper
{
  public  class AuthHelper
    {
    
        
        public static JwtWriteResponse WriteJwt(ApplicationUser user, IList<string> userRoles, AppSettings _appSettings)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
               // new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtWriteResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });

        }
    }

  public class JwtWriteResponse
  {
      public string Token { get; set; }
      public DateTime Expiration { get; set; }  
  }
}
