using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityService.Server.Controllers
{
    public class UserController : Controller
    {
        UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Register()
        {
            var result = await _userManager.CreateAsync(new ApplicationUser()
            {
                 Email = "me@me.com",
                 UserName = "me@me.com"                 
            }, password: "psswrd");
            return JsonConvert.SerializeObject(result);
        }
    }
}