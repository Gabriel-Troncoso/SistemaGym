using Microsoft.AspNetCore.Mvc;

namespace SistemaGym.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API funcionando correctamente 🚀");
        }
    }
}