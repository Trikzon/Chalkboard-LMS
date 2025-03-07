using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class ModuleController(ModuleService service) : ControllerBase
{
    [HttpPost("/courses/{courseId:guid}/modules")]
    public IActionResult CreateModule(Guid courseId, CreateModuleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest();
        }
        
        var module = new Module(Guid.NewGuid(), courseId, request.Name);
        if (!service.CreateModule(module))
        {
            return BadRequest();
        }

        return CreatedAtAction(
            actionName: nameof(GetModule),
            controllerName: "Module",
            routeValues: new { moduleId = module.Id },
            value: module
        );
    }
    
    [HttpGet("/modules/{moduleId:guid}")]
    public IActionResult GetModule(Guid moduleId)
    {
        var module = service.GetModule(moduleId);
        if (module is null)
        {
            return NotFound();
        }
        
        return Ok(module);
    }
    
    [HttpGet("/courses/{courseId:guid}/modules")]
    public IActionResult GetModules(Guid courseId)
    {
        return Ok(service.GetModules(courseId));
    }
    
    [HttpPut("/courses/{courseId:guid}/modules/{moduleId:guid}")]
    public IActionResult UpdateModule(Guid courseId, Guid moduleId, UpdateModuleRequest request)
    {
        var module = new Module(moduleId, courseId, request.Name);
        
        var existingModule = service.GetModule(moduleId);
        if (existingModule is null || existingModule.CourseId != courseId)
        {
            return BadRequest();
        }

        if (!service.UpdateModule(module))
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpDelete("/modules/{moduleId:guid}")]
    public IActionResult DeleteModule(Guid moduleId)
    {
        service.DeleteModule(moduleId);
        
        return NoContent();
    }
}