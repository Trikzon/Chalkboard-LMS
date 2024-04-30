using Lms.Library.Api.WebApi;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.Services;

public class StudentService
{
    private static StudentService? _instance;
    private static readonly object Lock = new();
    
    public static StudentService Current
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new StudentService(ApiUtils.HostUrl);
            }
        }
    }
    
    private readonly IStudentsApi _api;
    
    private StudentService(string hostUrl)
    {
        _api = RestService.For<IStudentsApi>(hostUrl);
    }
    
    public async Task<Student?> CreateStudentAsync(string name, Classification classification)
    {
        try
        {
            return await _api.CreateStudentAsync(new CreateStudentRequest(name, classification));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<IEnumerable<Student>?> GetStudentsAsync()
    {
        try
        {
            return await _api.GetStudentsAsync();
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<Student?> GetStudentAsync(Guid studentId)
    {
        try
        {
            return await _api.GetStudentAsync(studentId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateStudentAsync(Guid studentId, string name, Classification classification)
    {
        try
        {
            await _api.UpdateStudentAsync(studentId, new UpdateStudentRequest(name, classification));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task DeleteStudentAsync(Guid studentId)
    {
        try
        {
            await _api.DeleteStudentAsync(studentId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
}