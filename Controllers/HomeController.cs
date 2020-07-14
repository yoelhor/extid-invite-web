using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using extid_invite_web.Models;
using Microsoft.Extensions.Options;

namespace extid_invite_web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInviteService _inviteService;
        private readonly AppSettings _appSettings;

        public HomeController(ILogger<HomeController> logger, IInviteService inviteService, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _inviteService = inviteService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public ActionResult Index(string name, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                    _inviteService.InviteUser(_appSettings, name, email);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
