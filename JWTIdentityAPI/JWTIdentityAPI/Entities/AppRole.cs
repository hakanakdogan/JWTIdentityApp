using Microsoft.AspNetCore.Identity;
using System;

namespace JWTIdentityAPI.Entities
{
    public class AppRole : IdentityRole
    {
        public DateTime CreatedOn { get; set; }
    }
}
