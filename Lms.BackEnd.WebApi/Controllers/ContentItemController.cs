using Lms.BackEnd.WebApi.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lms.BackEnd.WebApi.Controllers;

[ApiController]
public class ContentItemController(ContentItemService service) : ControllerBase
{
    [HttpPost("/modules/{moduleId:guid}/content-items")]
    public IActionResult CreateContentItem(Guid moduleId, CreateContentItemRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest();
        }
        
        var contentItem = new ContentItem(Guid.NewGuid(), moduleId, request.Name, request.Content);
        if (!service.CreateContentItem(contentItem))
        {
            return BadRequest();
        }

        return CreatedAtAction(
            actionName: nameof(GetContentItem),
            controllerName: "ContentItem",
            routeValues: new { contentItemId = contentItem.Id },
            value: new ContentItemResponse(contentItem.Id, contentItem.ModuleId, contentItem.Name, contentItem.Content)
        );
    }
    
    [HttpPost("/modules/{moduleId:guid}/assignments")]
    public IActionResult CreateAssignment(Guid moduleId, CreateAssignmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest();
        }
        
        var assignment = new Assignment(Guid.NewGuid(), moduleId, request.Name, request.Content, request.TotalAvailablePoints, request.DueDate);
        if (!service.CreateContentItem(assignment))
        {
            return BadRequest();
        }

        return CreatedAtAction(
            actionName: nameof(GetContentItem),
            controllerName: "ContentItem",
            routeValues: new { contentItemId = assignment.Id },
            value: new AssignmentResponse(assignment.Id, assignment.ModuleId, assignment.Name, assignment.Content, assignment.TotalAvailablePoints, assignment.DueDate)
        );
    }
    
    [HttpGet("/content-items/{contentItemId:guid}")]
    public IActionResult GetContentItem(Guid contentItemId)
    {
        var contentItem = service.GetContentItem(contentItemId);
        if (contentItem is null)
        {
            return NotFound();
        }
        
        if (contentItem is Assignment assignment)
        {
            return Ok(new AssignmentResponse(assignment.Id, assignment.ModuleId, assignment.Name, assignment.Content, assignment.TotalAvailablePoints, assignment.DueDate));
        }
        
        return Ok(new ContentItemResponse(contentItem.Id, contentItem.ModuleId, contentItem.Name, contentItem.Content));
    }
    
    [HttpGet("/modules/{moduleId:guid}/content-items")]
    public IActionResult GetContentItems(Guid moduleId)
    {
        return Ok(service.GetContentItems(moduleId).Select(contentItem =>
        {
            if (contentItem is Assignment assignment)
            {
                return new AssignmentResponse(assignment.Id, assignment.ModuleId, assignment.Name, assignment.Content, assignment.TotalAvailablePoints, assignment.DueDate);
            }
            
            return new ContentItemResponse(contentItem.Id, contentItem.ModuleId, contentItem.Name, contentItem.Content);
        }));
    }
    
    [HttpPut("/modules/{moduleId:guid}/content-items/{contentItemId:guid}")]
    public IActionResult UpdateContentItem(Guid moduleId, Guid contentItemId, UpdateContentItemRequest request)
    {
        var contentItem = new ContentItem(contentItemId, moduleId, request.Name, request.Content);
        
        var existingContentItem = service.GetContentItem(contentItemId);
        if (existingContentItem is null || existingContentItem.ModuleId != moduleId)
        {
            return BadRequest();
        }

        if (!service.UpdateContentItem(contentItem))
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpPut("/modules/{moduleId:guid}/assignments/{contentItemId:guid}")]
    public IActionResult UpdateAssignment(Guid moduleId, Guid contentItemId, UpdateAssignmentRequest request)
    {
        var assignment = new Assignment(contentItemId, moduleId, request.Name, request.Content, request.TotalAvailablePoints, request.DueDate);
        
        var existingAssignment = service.GetContentItem(contentItemId) as Assignment;
        if (existingAssignment is null || existingAssignment.ModuleId != moduleId)
        {
            return BadRequest();
        }

        if (!service.UpdateContentItem(assignment))
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpDelete("/content-items/{contentItemId:guid}")]
    public IActionResult DeleteContentItem(Guid contentItemId)
    {
        service.DeleteContentItem(contentItemId);
        
        return NoContent();
    }
}