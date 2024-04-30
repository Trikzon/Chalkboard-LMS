using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class CourseController(CourseService service) : ControllerBase
{
    [HttpPost("/courses")]
    public IActionResult CreateCourse(CreateCourseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest();
        }
        
        var course = new Course(Guid.NewGuid(), request.Name, request.Code, request.Description);
        service.CreateCourse(course);

        return CreatedAtAction(
            nameof(GetCourse), 
            routeValues: new { courseId = course.Id }, 
            value: course
        );
    }
    
    [HttpGet("/courses")]
    public IActionResult GetCourse()
    {
        return Ok(service.GetCourses());
    }
    
    [HttpGet("/courses/{courseId:guid}")]
    public IActionResult GetCourse(Guid courseId)
    {
        var course = service.GetCourse(courseId);
        if (course is null)
        {
            return NotFound();
        }
        
        return Ok(course);
    }
    
    [HttpPut("/courses/{courseId:guid}")]
    public IActionResult UpdateCourse(Guid courseId, UpdateCourseRequest request)
    {
        var course = new Course(courseId, request.Name, request.Code, request.Description);

        if (!service.UpdateCourse(course))
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpDelete("/courses/{courseId:guid}")]
    public IActionResult DeleteCourse(Guid courseId)
    {
        service.DeleteCourse(courseId);
        
        return NoContent();
    }
}