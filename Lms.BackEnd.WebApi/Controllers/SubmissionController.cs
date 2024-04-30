using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class SubmissionController(SubmissionService service) : ControllerBase
{
    [HttpPost("/submissions/{contentItemId:guid}/{studentId:guid}")]
    public IActionResult CreateSubmission(Guid contentItemId, Guid studentId, CreateSubmissionRequest request)
    {
        var submission = new Submission(contentItemId, studentId, request.Content, request.SubmissionDate, request.Points);
        if (!service.CreateSubmission(submission))
        {
            return BadRequest();
        }

        return CreatedAtAction(
            actionName: nameof(GetSubmission),
            controllerName: "Submission",
            routeValues: new { contentItemId, studentId },
            value: new SubmissionResponse(submission.ContentItemId, submission.StudentId, submission.Content, submission.SubmissionDate, submission.Points)
        );
    }
    
    [HttpGet("/submissions/{contentItemId:guid}/{studentId:guid}")]
    public IActionResult GetSubmission(Guid contentItemId, Guid studentId)
    {
        var submission = service.GetSubmission(contentItemId, studentId);
        if (submission is null)
        {
            return NotFound();
        }

        return Ok(new SubmissionResponse(submission.ContentItemId, submission.StudentId, submission.Content, submission.SubmissionDate, submission.Points));
    }
    
    [HttpPut("/submissions/{contentItemId:guid}/{studentId:guid}")]
    public IActionResult UpdateSubmission(Guid contentItemId, Guid studentId, UpdateSubmissionRequest request)
    {
        var submission = new Submission(contentItemId, studentId, request.Content, request.SubmissionDate, request.Points);
        if (!service.UpdateSubmission(submission))
        {
            return NotFound();
        }

        return Ok(new SubmissionResponse(submission.ContentItemId, submission.StudentId, submission.Content, submission.SubmissionDate, submission.Points));
    }
    
    [HttpDelete("/submissions/{contentItemId:guid}/{studentId:guid}")]
    public IActionResult DeleteSubmission(Guid contentItemId, Guid studentId)
    {
        if (!service.DeleteSubmission(contentItemId, studentId))
        {
            return NotFound();
        }

        return NoContent();
    }
}