using Microsoft.AspNetCore.Mvc;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    public object Health()
    {
        return new { status = "healthy", service = "UsersService" };
    }
}