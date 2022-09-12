using System.ComponentModel.DataAnnotations;

namespace JWTIdentityAPI.Entities.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
