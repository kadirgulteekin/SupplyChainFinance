using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SupplyChainFinance.IdentityServver.Dto;
using SupplyChainFinance.IdentityServver.Models;
using System.Threading;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;


namespace SupplyChainFinance.IdentityServver.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var user = new ApplicationUser
            {
                UserName = signUpDto.UserName,
                City = signUpDto.City,
                Email = signUpDto.Email,

            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("User couldnt create!");
            }
            
            return Ok(result);
        }
    }
}
