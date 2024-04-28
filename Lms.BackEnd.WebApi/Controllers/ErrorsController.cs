using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class ErrorsController : ControllerBase
{
    [HttpGet("/error")]
    public IActionResult Error() => Problem();
}