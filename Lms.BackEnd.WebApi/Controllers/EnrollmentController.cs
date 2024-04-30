using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class EnrollmentController(EnrollmentService service) : ControllerBase
{
    [HttpPost("/enrollments")]
    public IActionResult CreateEnrollment(CreateEnrollmentRequest request)
    {
        if (!service.CreateEnrollment(request.CourseId, request.StudentId))
        {
            return BadRequest();
        }

        return CreatedAtAction(
            nameof(GetEnrollment),
            routeValues: new { studentId = request.StudentId, courseId = request.CourseId },
            value: new EnrollmentResponse(request.CourseId, request.StudentId)
        );
    }
    
    [HttpGet("/enrollments")]
    public IActionResult GetEnrollment([FromQuery] Guid courseId, [FromQuery] Guid studentId)
    {
        if (!service.EnrollmentExists(courseId, studentId))
        {
            return NotFound();
        }
        
        return Ok(new EnrollmentResponse(courseId, studentId));
    }
    
    [HttpGet("/students/{studentId:guid}/enrollments")]
    public IActionResult GetEnrolledCourses(Guid studentId)
    {
        return Ok(service.GetEnrolledCourses(studentId).Select(courseId => new EnrollmentResponse(courseId, studentId)));
    }
    
    [HttpGet("/courses/{courseId:guid}/enrollments")]
    public IActionResult GetEnrolledStudents(Guid courseId)
    {
        return Ok(service.GetEnrolledStudents(courseId).Select(studentId => new EnrollmentResponse(courseId, studentId)));
    }
    
    [HttpDelete("/enrollments")]
    public IActionResult DeleteEnrollment([FromQuery] Guid courseId, [FromQuery] Guid studentId)
    {
        service.DeleteEnrollment(courseId, studentId);
        
        return NoContent();
    }
}