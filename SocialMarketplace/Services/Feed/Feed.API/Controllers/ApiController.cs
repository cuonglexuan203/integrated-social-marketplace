using Microsoft.AspNetCore.Mvc;

namespace Feed.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
    }
}
