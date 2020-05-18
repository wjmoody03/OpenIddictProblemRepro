using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Server.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "This is the identity server. We know who you are.";
        }
    }
}