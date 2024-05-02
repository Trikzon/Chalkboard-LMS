using Lms.Library.Api.WebApi;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.Services;

public class SubmissionService
{
    private static SubmissionService? _instance;
    private static readonly object Lock = new();
    
    public static SubmissionService Current
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new SubmissionService(ApiUtils.HostUrl);
            }
        }
    }
    
    private readonly ISubmissionsApi _api;
    
    private SubmissionService(string hostUrl)
    {
        _api = RestService.For<ISubmissionsApi>(hostUrl);
    }
    
    public async Task<Submission?> CreateSubmissionAsync(Guid contentItemId, Guid studentId, string? content, DateTime submissionDate, float points)
    {
        try
        {
            return await _api.CreateSubmissionAsync(contentItemId, studentId, new CreateSubmissionRequest(content, submissionDate, points));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<IList<Submission>?> GetSubmissionsAsync(Guid contentItemId)
    {
        try
        {
            return await _api.GetSubmissionsAsync(contentItemId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<Submission?> GetSubmissionAsync(Guid contentItemId, Guid studentId)
    {
        try
        {
            return await _api.GetSubmissionAsync(contentItemId, studentId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateSubmissionAsync(Guid contentItemId, Guid studentId, string? content, DateTime submissionDate, float points)
    {
        try
        {
            await _api.UpdateSubmissionAsync(contentItemId, studentId, new UpdateSubmissionRequest(content, submissionDate, points));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
}