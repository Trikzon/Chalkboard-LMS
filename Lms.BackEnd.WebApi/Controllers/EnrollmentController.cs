using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class EnrollmentController(EnrollmentService service) : ControllerBase
{
    [HttpPost("/courses/{courseId:guid}/enrollments/{studentId:guid}")]
    [HttpPost("/students/{studentId:guid}/enrollments/{courseId:guid}")]
    public IActionResult CreateEnrollment(Guid studentId, Guid courseId)
    {
        if (!service.CreateEnrollment(courseId, studentId))
        {
            return BadRequest();
        }

        return Ok();
    }
    
    [HttpGet("/students/{studentId:guid}/enrollments")]
    public IActionResult GetEnrolledCourses(Guid studentId)
    {
        return Ok(service.GetEnrolledCourses(studentId));
    }
    
    [HttpGet("/courses/{courseId:guid}/enrollments")]
    public IActionResult GetEnrolledStudents(Guid courseId)
    {
        return Ok(service.GetEnrolledStudents(courseId));
    }
    
    [HttpDelete("/courses/{courseId:guid}/enrollments/{studentId:guid}")]
    [HttpDelete("/students/{studentId:guid}/enrollments/{courseId:guid}")]
    public IActionResult DeleteEnrollment(Guid studentId, Guid courseId)
    {
        service.DeleteEnrollment(courseId, studentId);
        
        return NoContent();
    }
}