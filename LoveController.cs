using LoveApp.Lib;
using System.Runtime.InteropServices;
using System.Web.Http;

namespace LoveApp.Web.Controllers
{
    [RoutePrefix("api/love")]
    public class LoveController : ApiController
    {
        [HttpGet]
        [Route("calculate")]
        public IHttpActionResult Calculate(string name1, string name2)
        {
            var calc = new LoveCalculator();
            int score = calc.CalculateLove(name1 ?? "", name2 ?? "");
            string message = calc.GetMessage(score);
            return Ok(new { name1, name2, score, message });
        }
    }
}
