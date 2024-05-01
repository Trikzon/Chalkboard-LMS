using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.WebApi;

public interface IModulesApi
{
    [Get("/modules/{moduleId}")]
    Task<Module> GetModuleAsync(Guid moduleId);
    
    [Delete("/modules/{moduleId}")]
    Task DeleteModuleAsync(Guid moduleId);
    
    [Post("/modules/{moduleId}/content-items")]
    Task<ContentItem> CreateContentItemAsync(Guid moduleId, CreateContentItemRequest request);
    
    [Get("/modules/{moduleId}/content-items")]
    Task<IEnumerable<ContentItem>> GetContentItemsAsync(Guid moduleId);
    
    [Put("/modules/{moduleId}/content-items/{contentItemId}")]
    Task UpdateContentItemAsync(Guid moduleId, Guid contentItemId, UpdateContentItemRequest request);
    
    [Get("/modules/{moduleId}/assignments")]
    Task<IEnumerable<Assignment>> GetAssignmentsAsync(Guid moduleId);
}