using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TestsWebNet6.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IOptionsSnapshot<Ftp> ftpOpt;
        private readonly IOptionsSnapshot<Cors> corsOpt;

        public TestController(IOptionsSnapshot<Ftp> ftpOpt, IOptionsSnapshot<Cors> corsOpt)
        {
            this.ftpOpt = ftpOpt;
            this.corsOpt = corsOpt;
        }

        [HttpGet]
        public string Test1()
        {
            return ftpOpt.Value + ""+corsOpt.Value;
        }
    }
}
