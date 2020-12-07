﻿using Day4InClass.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Day4InClass.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IServiceProvider _serviceProvider;

        public AuthController(
            IConfiguration config, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IServiceProvider serviceProvider)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;

        }

        [HttpPost("Register")]
        public async Task<JsonResult> RegisterAsync([FromBody] RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                //will need to implement email or turn off email verification
                var user = new IdentityUser { UserName = registerVM.Email, Email = registerVM.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    return await SignInAsync(new LoginVM() { Email = registerVM.Email, Password = registerVM.Password });
                }
            }
            dynamic jsonResponse = new JObject();
            jsonResponse.token = "";
            jsonResponse.status = "Registration Failed";
            return Json(jsonResponse);
        }

        [HttpPost("Login")]
        public async Task<JsonResult> SignInAsync([FromBody] LoginVM loginVM)
        {
            dynamic jsonResponse = new JObject();
            if (ModelState.IsValid)
            {
                var result = await
                            _signInManager.PasswordSignInAsync(loginVM.Email.ToUpper(),
                            loginVM.Password, loginVM.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(loginVM.Email);

                    if (user != null)
                    {
                        var tokenString = GenerateJSONWebToken(user);
                        jsonResponse.token = tokenString;
                        jsonResponse.status = "OK";
                        return Json(jsonResponse);
                    }
                }
                else if (result.IsLockedOut)
                {
                    jsonResponse.token = "";
                    jsonResponse.status = "Locked Out";
                    return Json(jsonResponse);
                }
            }
            jsonResponse.token = "";
            jsonResponse.status = "Invalid Login";
            return Json(jsonResponse);
        }

        string GenerateJSONWebToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
                // Add roles here
            };

            // A security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_SITEKEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWT_ISSUER"],
                _config["JWT_ISSUER"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
