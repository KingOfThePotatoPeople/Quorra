using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using OpenIddict.Server.AspNetCore;

namespace Quorra.Controllers
{
    
    [Produces("application/json")]
    [Route("api/log")]
    [ApiController]
    public class LogController : Controller
    {


        private readonly Logger _logger = LogManager.GetCurrentClassLogger();


        // ----------------------------------------------------------------
        //  Get Auth Logs
        // ----------------------------------------------------------------

        [HttpGet]
        [Route("auth")]
        public IActionResult GetAuthLogs()
        {
            try
            {
                return Ok("Hello");
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return StatusCode(500, e.Message);
            }
        }


    }
}