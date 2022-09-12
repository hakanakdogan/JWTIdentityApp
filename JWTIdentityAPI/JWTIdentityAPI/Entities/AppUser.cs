using Microsoft.AspNetCore.Identity;
using System;

namespace JWTIdentityAPI.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedOn { get; set; }
    }
}
