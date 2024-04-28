using Lms.BackEnd.WebApi.Contracts;
using Lms.BackEnd.WebApi.Models;
using Lms.BackEnd.WebApi.Services;
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
            value: new StudentResponse(student.Id, student.Name, student.Classification)
        );
    }
    
    [HttpGet("students")]
    public IActionResult GetStudent([FromQuery] Guid? studentId)
    {
        if (studentId is null)
        {
            return Ok(service.GetStudents().Select(student => new StudentResponse(student.Id, student.Name, student.Classification)));
        }
        
        var student = service.GetStudent(studentId.Value);
        if (student is null)
        {
            return NotFound();
        }
        
        return Ok(new StudentResponse(student.Id, student.Name, student.Classification));
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