using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class ApiController: ControllerBase
    {
    }
}
