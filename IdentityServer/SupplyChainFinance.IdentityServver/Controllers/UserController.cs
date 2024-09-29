using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SupplyChainFinance.IdentityServver.Dto;
using SupplyChainFinance.IdentityServver.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public class Response<T>
        {
            public bool Success { get; set; }
            public List<string> Errors { get; set; }
            public int StatusCode { get; set; }

            public static Response<T> Fail(List<string> errors, int statusCode)
            {
                return new Response<T> { Success = false, Errors = errors, StatusCode = statusCode };
            }
        }
        public class NoContent
        {

        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signupDto)
        {
            var user = new ApplicationUser();
            user.UserName = signupDto.UserName;
            user.Email = signupDto.Email;
            user.City = signupDto.City;

            var result = await _userManager.CreateAsync(user, signupDto.Password);

            if (!result.Succeeded)
            {
                var response = Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400);
                return BadRequest(response);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null) return BadRequest();

            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City });
        }
    }
}
