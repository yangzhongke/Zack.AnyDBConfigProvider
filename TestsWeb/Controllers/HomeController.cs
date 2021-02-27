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

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IOptionsSnapshot<Ftp> ftpOpt)
        {
            _logger = logger;
            this.config = config;
            this.ftpOpt = ftpOpt;
        }

        public IActionResult Index()
        {
            string[] strs = config.GetSection("Cors:Origins").Get<string[]>();
            string s = string.Join("|", strs);
            ViewBag.s = s;
            ViewBag.ftp = ftpOpt.Value;
            return View();
        }
    }
}
