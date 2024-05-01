using Lms.Library.Api.WebApi;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.Services;

public class ModuleService
{
    private static ModuleService? _instance;
    private static readonly object Lock = new();
    
    public static ModuleService Current
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new ModuleService(ApiUtils.HostUrl);
            }
        }
    }
    
    private readonly IModulesApi _api;
    
    private ModuleService(string hostUrl)
    {
        _api = RestService.For<IModulesApi>(hostUrl);
    }
    
    public async Task<Module?> GetModuleAsync(Guid moduleId)
    {
        try
        {
            return await _api.GetModuleAsync(moduleId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task DeleteModuleAsync(Guid moduleId)
    {
        try
        {
            await _api.DeleteModuleAsync(moduleId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }

    public async Task<ContentItem?> CreateContentItemAsync(Guid moduleId, string name, string? content = null)
    {
        try
        {
            return await _api.CreateContentItemAsync(moduleId, new CreateContentItemRequest(name, content));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<IEnumerable<ContentItem>?> GetContentItemsAsync(Guid moduleId)
    {
        try
        {
            return await _api.GetContentItemsAsync(moduleId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateContentItemAsync(Guid moduleId, Guid contentItemId, string name, string? content = null)
    {
        try
        {
            await _api.UpdateContentItemAsync(moduleId, contentItemId, new UpdateContentItemRequest(name, content));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task<IEnumerable<Assignment>?> GetAssignmentsAsync(Guid moduleId)
    {
        try
        {
            return await _api.GetAssignmentsAsync(moduleId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
}