using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryWebApi.DAL;
using LibraryWebApi.DTO;
using LibraryWebApi.Enums;
using LibraryWebApi.Helpers;
using LibraryWebApi.Helpers.Responses;
using LibraryWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LibraryWebApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IDatabase _database;
        private readonly JwtConfig _jwtConfig;

        public AuthController(IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _database = new UsersDatabase();
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] NewUserRequest newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Something went wrong... Please try again later."
                    }
                });
            
            if (((UsersDatabase) _database).GetUserByEmail(newUser.Email) != null)
            {
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "User with this email has been already registered."
                    }
                });
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = newUser.Email,
                FirstName = newUser.Firstname,
                LastName = newUser.Lastname,
                Password = newUser.Password,
                Role = Role.User
            };

            if (newUser.Password.Equals("Administrator12a@")) user.Role = Role.Admin;

            var token = GenerateJwtToken(user);

            _database.Create(user);
            _database.SaveDatabase();

            return Ok(new LogInResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Registered successfully!"
                },
                Token = token
            });

        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Something went wrong... Please try again later."
                    }
                });
            var user = ((UsersDatabase) _database).GetUserByEmail(loginUser.Email);
            if (user == null)
            {
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "User with this email not registered yet."
                    }
                });
            }

            if (!user.Password.Equals(loginUser.Password))
            {
                return BadRequest(new ActionResponse()
                {
                    Result = false,
                    Messages = new List<string>()
                    {
                        "Wrong password"
                    }
                });
            }

            var token = GenerateJwtToken(user);

            return Ok(new LogInResponse()
            {
                Result = true,
                Messages = new List<string>()
                {
                    "Logged in successfully!"
                },
                Token = token
            });

        }
        
        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRole = user.Role;
            claims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        
    }
}
