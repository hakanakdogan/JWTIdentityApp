using System;

namespace JWTIdentityAPI.Entities.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
