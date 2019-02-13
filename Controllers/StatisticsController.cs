using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MeteostationService
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class StatisticsController : Controller
    {
        public const double DefaultSampleSize = 0.1;
        private StatisticsService _statisticsService;
        private UserManager<ApplicationUser> _userManager;

        public StatisticsController(
            StatisticsService statisticsService,
            UserManager<ApplicationUser> userManager
        )
        {
            _statisticsService = statisticsService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> SendStats()
        {
            var temperature = Request.Form["temp"].ToString();
            var humidity = Request.Form["humidity"].ToString();
            var time = Request.Form["time"].ToString();
            var measurementTime = DateTime.ParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return StatusCode(500);
            }

            _statisticsService.WriteStatistics(temperature, humidity, measurementTime, user);
            return Ok();
        }

        [AllowAnonymous]
        public ActionResult<List<StatisticsResult>> GetStats()
        {
            var sampleSize = string.IsNullOrWhiteSpace(Request.Query["sampleSize"]) ? 
                DefaultSampleSize :
                Double.Parse(Request.Query["sampleSize"]);

            var userName = Request.Query["userName"];

            var startTime = DateTime.ParseExact(
                Request.Query["startTime"].ToString(),
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture
            );
            var endTime = DateTime.ParseExact(
                Request.Query["endTime"].ToString(),
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture
            );
            
            return Json(_statisticsService.GetStatistics(startTime, endTime, userName, sampleSize));
        }
        
        [AllowAnonymous]
        public JsonResult GetUsers()
        {
            return Json(_statisticsService.GetUsers()); 
        }

    }
}