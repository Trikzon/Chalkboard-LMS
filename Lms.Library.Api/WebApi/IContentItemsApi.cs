using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.WebApi;

public interface IContentItemsApi
{
    [Get("/content-items/{contentItemId}")]
    Task<ContentItem?> GetContentItemAsync(Guid contentItemId);
    
    [Delete("/content-items/{contentItemId}")]
    Task DeleteContentItemAsync(Guid contentItemId);
    
    [Get("/assignments/{contentItemId}")]
    Task<Assignment?> GetAssignmentAsync(Guid contentItemId);
}