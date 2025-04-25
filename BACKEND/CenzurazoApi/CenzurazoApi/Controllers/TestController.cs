using Microsoft.AspNetCore.Mvc;
namespace CenzurazoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API működik!");
        }
    }
}
