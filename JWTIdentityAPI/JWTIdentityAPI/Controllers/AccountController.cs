using JWTIdentityAPI.Entities;
using JWTIdentityAPI.Entities.Dtos;
using JWTIdentityAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWTIdentityAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if(user == null) return Unauthorized();
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded) 
            {
                return new UserDto { Email = user.Email, UserName = user.UserName, CreatedOn = DateTime.UtcNow, Token = _tokenService.CreateToken(user).Result };
            }

            return Unauthorized();
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(x=>x.Email == registerDto.Email))
            {

                return BadRequest("Email taken");
            }

            if(await _userManager.Users.AnyAsync(_ =>_.UserName == registerDto.UserName))
            {
                return BadRequest("Username Taken");
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                CreatedOn = DateTime.UtcNow

            };

            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (res.Succeeded)
            {
                return new UserDto
                {
                    CreatedOn = user.CreatedOn,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user).Result

                };
            }

            return BadRequest("Something went wrong");
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<UserDtoWithId>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return new UserDtoWithId
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user).Result,
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
