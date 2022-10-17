using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _configuration;
        //private readonly JwtConfig _jwtConfig;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration
            //JwtConfig jwtConfig,
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            //_jwtConfig = jwtConfig;
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
                    var token = GenerateJwtToken(new_user);

                    return Ok(new AuthenticationResults()
                    {
                        Results = true,
                        Token = token
                    });
                }

                return BadRequest(new AuthenticationResults()
                {
                    Errors = new List<string>()
                    {
                        "Server error"
                    },
                    Results = false
                });

            }

            return BadRequest();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if(ModelState.IsValid)
            {
                var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if(existing_user == null)
                    return BadRequest(new AuthenticationResults()
                    {
                        Errors = new List<string>()
                        {
                            "Isvalid payload"
                        },
                        Results = false
                    });

                var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);
                if(!isCorrect)
                    return BadRequest(new AuthenticationResults()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid "
                        },
                        Results = false
                    });

                var jwtToken = GenerateJwtToken(existing_user);
                return Ok(new AuthenticationResults()
                {
                    Token = jwtToken,
                    Results = true
                });
            }

            return BadRequest(new AuthenticationResults
            {
                Errors = new List<string>
                {
                    "Isvalid paylod"
                },
                Results = false

            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject =  new ClaimsIdentity(new[] 
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}