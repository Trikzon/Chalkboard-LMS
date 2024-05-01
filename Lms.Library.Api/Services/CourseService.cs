using Lms.Library.Api.WebApi;
using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.Services;

public class CourseService
{
    private static CourseService? _instance;
    private static readonly object Lock = new();

    public static CourseService Current
    {
        get
        {
            lock (Lock)
            {
                return _instance ??= new CourseService(ApiUtils.HostUrl);
            }
        }
    }

    private readonly ICoursesApi _api;

    private CourseService(string hostUrl)
    {
        _api = RestService.For<ICoursesApi>(hostUrl);
    }
    
    public async Task<Course?> CreateCourseAsync(string name, string course, string? description = null)
    {
        try
        {
            return await _api.CreateCourseAsync(new CreateCourseRequest(name, course, description));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }

    public async Task<IEnumerable<Course>?> GetCoursesAsync()
    {
        try
        {
            return await _api.GetCoursesAsync();
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<Course?> GetCourseAsync(Guid courseId)
    {
        try
        {
            return await _api.GetCourseAsync(courseId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateCourseAsync(Guid courseId, string name, string code, string? description = "")
    {
        try
        {
            await _api.UpdateCourseAsync(courseId, new UpdateCourseRequest(name, code, description));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task CreateEnrollmentAsync(Guid courseId, Guid studentId)
    {
        try
        {
            await _api.CreateEnrollmentAsync(courseId, studentId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task<IEnumerable<Student>?> GetEnrolledStudentsAsync(Guid courseId)
    {
        try
        {
            return await _api.GetEnrolledStudentsAsync(courseId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task DeleteEnrollmentAsync(Guid courseId, Guid studentId)
    {
        try
        {
            await _api.DeleteEnrollmentAsync(courseId, studentId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task DeleteCourseAsync(Guid courseId)
    {
        try
        {
            await _api.DeleteCourseAsync(courseId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
    
    public async Task<Module?> CreateModuleAsync(Guid courseId, string name)
    {
        try
        {
            return await _api.CreateModuleAsync(courseId, new CreateModuleRequest(name));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task<IEnumerable<Module>?> GetModulesAsync(Guid courseId)
    {
        try
        {
            return await _api.GetModulesAsync(courseId);
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateModuleAsync(Guid courseId, Guid moduleId, string name)
    {
        try
        {
            await _api.UpdateModuleAsync(courseId, moduleId, new UpdateModuleRequest(name));
        }
        catch (ApiException exception)
        {
            ApiUtils.HandleApiException(exception);
        }
    }
}