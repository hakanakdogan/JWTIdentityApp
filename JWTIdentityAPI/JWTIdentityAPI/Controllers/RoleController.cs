using JWTIdentityAPI.Entities;
using JWTIdentityAPI.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JWTIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        [HttpPost("Createrole")]
        public async Task<ActionResult<RoleDto>> UpsertRole(UpsertRoleDto upsertRoleDto)
        {
            var isUpdate = upsertRoleDto.Id != null;

            var role = isUpdate ? await _roleManager.FindByIdAsync(upsertRoleDto.Id) : new AppRole { Name = upsertRoleDto.Name, CreatedOn = DateTime.UtcNow };

            var res = isUpdate ? await _roleManager.UpdateAsync(role) : await _roleManager.CreateAsync(role);

           

            if (res.Succeeded)
            {
                return new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    CreatedOn = role.CreatedOn
                };
            }
            return BadRequest("Something went wrong");

        }

        //[HttpPost("Assingrole")]
        //public async Task<ActionResult> AssingRole(string userId, string roleName)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) return BadRequest("User couldn't be found");
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    if (role == null) return BadRequest("Role couldn't be found");

        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    foreach (var item in userRoles)
        //    {
                
        //    }
        //}
            


    }
}
