using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeteostationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly StatisticsService _statisticsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            StatisticsService statisticsService, 
            UserManager<ApplicationUser> userManager
        )
        {
            _signInManager = signInManager;
            _statisticsService = statisticsService;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login()
        {
            var username = Request.Form["login"].First();
            var password = Request.Form["password"].First();
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            var username = Request.Form["login"].First();
            var password = Request.Form["password"].First();
            if ((await _userManager.FindByNameAsync(username)) != null)
            {
                return StatusCode(409);
            }
            var user = new ApplicationUser() { UserName = username };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                return Ok();
            else
                return StatusCode(500);
        }
    }
}
