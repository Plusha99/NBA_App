using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NBA_App.Configuration;
using NBA_App.Models;
using NBA_App.Models.Dto;

namespace NBA_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            JwtConfig jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrstionRequestDto requestDto)
        {
            if(ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

                if(user_exist != null)
                {
                    return BadRequest(new AuthenticationResults()
                    {
                        Results = false,
                        Errors = new List<string>()
                        {
                            "Email already "
                        }
                    });
                }
                var new_user = new IdentityUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Email
                };

                var is_created = await _userManager.CreateAsync(new_user, requestDto.Password);

                if(is_created.Succeeded)
                {

                }

                return BadRequest(new AuthenticationResults()
                {
                    Errors = new List<string>()
                    {
                        "Server error"
                    }
                    Results = false
                });

            }

            return BadRequest();
        }

        private readonly GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
        }
    }
}