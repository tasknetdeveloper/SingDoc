﻿using Microsoft.AspNetCore.Mvc;      
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Model;

namespace SigningDoc.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        
        public IConfiguration _configuration;
     //   private readonly DatabaseContext _context;

        public TokenController(IConfiguration config)//, DatabaseContext context)
        {
            _configuration = config;
           // _context = context;
        }

        private async Task<User> GetTestUser(User _userData)
        {
            return await new Task<User>(() => {
                return new User
                {
                    CreatedDate = DateTime.Now,
                    Email = "string",
                    Password = "string",
                    id = 0,
                    UserName = "string"
                };
            });
            
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(User _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetTestUser(_userData);//await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("id", user.id.ToString()),                      
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        //private async Task<User> GetUser(string email, string password)
        //{
        //    return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        //}
        
    }

}
