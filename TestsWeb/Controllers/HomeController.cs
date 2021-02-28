using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;
        private readonly IOptionsSnapshot<Ftp> ftpOpt;
        private readonly IOptionsSnapshot<Cors> corsOpt;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IOptionsSnapshot<Ftp> ftpOpt, IOptionsSnapshot<Cors> corsOpt)
        {
            _logger = logger;
            this.config = config;
            this.ftpOpt = ftpOpt;
            this.corsOpt = corsOpt;
        }

        public IActionResult Index()
        {
            string redisCS = config.GetSection("RedisConnStr").Get<string>();
            ViewBag.s = redisCS;
            ViewBag.ftp = ftpOpt.Value;
            ViewBag.cors = corsOpt.Value;
            return View();
        }
    }
}
