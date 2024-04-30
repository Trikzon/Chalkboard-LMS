using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class StudentController(StudentService service) : ControllerBase
{
    [HttpPost("/students")]
    public IActionResult CreateStudent(CreateStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest();
        }
        
        var student = new Student(Guid.NewGuid(), request.Name, request.Classification);
        service.CreateStudent(student);

        return CreatedAtAction(
            nameof(GetStudent), 
            routeValues: new { studentId = student.Id }, 
            value: student
        );
    }
    
    [HttpGet("/students")]
    public IActionResult GetStudent()
    {
        return Ok(service.GetStudents());
    }
    
    [HttpGet("/students/{studentId:guid}")]
    public IActionResult GetStudent(Guid studentId)
    {
        var student = service.GetStudent(studentId);
        if (student is null)
        {
            return NotFound();
        }
        
        return Ok(student);
    }
    
    [HttpPut("/students/{studentId:guid}")]
    public IActionResult UpdateStudent(Guid studentId, UpdateStudentRequest request)
    {
        var student = new Student(studentId, request.Name, request.Classification);

        if (!service.UpdateStudent(student))
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpDelete("/students/{studentId:guid}")]
    public IActionResult DeleteStudent(Guid studentId)
    {
        service.DeleteStudent(studentId);
        
        return NoContent();
    }
}