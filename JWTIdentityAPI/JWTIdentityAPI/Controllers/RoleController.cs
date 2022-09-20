using JWTIdentityAPI.Entities;
using JWTIdentityAPI.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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


        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("Assingrole")]
        public async Task<ActionResult> AssingRole(AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
            if (user == null) return BadRequest("User couldn't be found");
            var role = await _roleManager.FindByNameAsync(assignRoleDto.RoleName);
            if (role == null) return BadRequest("Role couldn't be found");

            var userRoles = await _userManager.GetRolesAsync(user);
            
            foreach (var item in userRoles)
            {
                if (item == assignRoleDto.RoleName)
                {
                    return BadRequest("User already has the role");
                }
            }

            await _userManager.AddToRoleAsync(user, assignRoleDto.RoleName);
            return Ok(new {Role = assignRoleDto.RoleName, User = user.UserName});
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("RemoveUserFromRole")]

        public async Task<ActionResult> RemoveUserFromRole(AssignRoleDto assignRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
            if (user == null) return BadRequest("User cannot be found");
            var role = await _roleManager.FindByNameAsync(assignRoleDto.RoleName);
            if (role == null) return BadRequest("Role cannot be found");

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                if(userRole == assignRoleDto.RoleName)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                    return Ok("User removed from role successfully");
                }
            }

            return BadRequest("Something went wrong");
        }

        [Authorize]
        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<string>>> GetUserRoles()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles == null || userRoles.Count < 1) return BadRequest("No roles found");

            return Ok(userRoles);

        }



    }
}
