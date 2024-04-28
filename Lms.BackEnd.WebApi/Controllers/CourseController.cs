using Lms.BackEnd.WebApi.Contracts;
using Lms.BackEnd.WebApi.Models;
using Lms.BackEnd.WebApi.Services;
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
            value: new CourseResponse(course.Id, course.Name, course.Code, course.Description)
        );
    }
    
    [HttpGet("/courses")]
    public IActionResult GetCourse([FromQuery] Guid? courseId)
    {
        if (courseId is null)
        {
            return Ok(service.GetCourses().Select(course => new CourseResponse(course.Id, course.Name, course.Code, course.Description)));
        }
        
        var course = service.GetCourse(courseId.Value);
        if (course is null)
        {
            return NotFound();
        }
        
        return Ok(new CourseResponse(course.Id, course.Name, course.Code, course.Description));
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