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

    public class UserDtoWithId
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
