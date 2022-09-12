using JWTIdentityAPI.Data;
using JWTIdentityAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTIdentityAPI
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context,UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{Email="hakan@example.com", UserName="akdoganhakan", CreatedOn = DateTime.UtcNow},
                    new AppUser{Email="betul@example.com", UserName="kanmazbetul", CreatedOn = DateTime.UtcNow},

                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "User.123");
                }

                await context.SaveChangesAsync();
            }

        }
    }
}
