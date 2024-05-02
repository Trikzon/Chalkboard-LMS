using Lms.Library.Api.WebApi;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.Services;

public class ContentItemService
{
    private static ContentItemService? _instance;
    private static readonly object Lock = new();
    
    public static ContentItemService Current
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new ContentItemService(ApiUtils.HostUrl);
            }
        }
    }
    
    private readonly IContentItemsApi _api;
    
    private ContentItemService(string hostUrl)
    {
        _api = RestService.For<IContentItemsApi>(hostUrl);
    }
    
    public async Task<ContentItem?> GetContentItemAsync(Guid contentItemId)
    {
        try
        {
            return await _api.GetContentItemAsync(contentItemId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task DeleteContentItemAsync(Guid contentItemId)
    {
        try
        {
            await _api.DeleteContentItemAsync(contentItemId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task<Assignment?> GetAssignmentAsync(Guid contentItemId)
    {
        try
        {
            return await _api.GetAssignmentAsync(contentItemId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
}