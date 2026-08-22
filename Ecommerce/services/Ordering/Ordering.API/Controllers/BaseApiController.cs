using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace Catalog.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
