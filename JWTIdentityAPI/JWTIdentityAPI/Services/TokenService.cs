using JWTIdentityAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTIdentityAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _config = config;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            
            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            AddRolesToClaims(claims, roles);

            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["IdentityAPI:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        private void AddRolesToClaims(List<Claim> claims, IList<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
    }
}
