using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.WebApi;

public interface ISubmissionsApi
{
    [Post("/submissions/{contentItemId}/{studentId}")]
    Task<Submission?> CreateSubmissionAsync(Guid contentItemId, Guid studentId, CreateSubmissionRequest submission);
    
    [Get("/submissions/{contentItemId}")]
    Task<List<Submission>> GetSubmissionsAsync(Guid contentItemId);
    
    [Get("/submissions/{contentItemId}/{studentId}")]
    Task<Submission?> GetSubmissionAsync(Guid contentItemId, Guid studentId);
    
    [Put("/submissions/{contentItemId}/{studentId}")]
    Task UpdateSubmissionAsync(Guid contentItemId, Guid studentId, UpdateSubmissionRequest submission);
    
    [Delete("/submissions/{contentItemId}/{studentId}")]
    Task DeleteSubmissionAsync(Guid contentItemId, Guid studentId);
}