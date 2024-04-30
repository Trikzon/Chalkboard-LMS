using System.Net;
using System.Runtime.CompilerServices;
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
                return _instance ??= new CourseService("http://localhost:5169");
            }
        }
    }

    private readonly ICoursesApi _api;

    private CourseService(string hostUrl)
    {
        _api = RestService.For<ICoursesApi>(hostUrl);
    }
    
    public async Task<Course?> CreateCourseAsync(CreateCourseRequest request)
    {
        try
        {
            return await _api.CreateCourseAsync(request);
        }
        catch (ApiException exception)
        {
            HandleApiException(exception);
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
            HandleApiException(exception);
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
            HandleApiException(exception);
            return null;
        }
    }
    
    public async Task UpdateCourseAsync(Guid courseId, UpdateCourseRequest request)
    {
        try
        {
            await _api.UpdateCourseAsync(courseId, request);
        }
        catch (ApiException exception)
        {
            HandleApiException(exception);
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
            HandleApiException(exception);
        }
    }

    private static void HandleApiException(ApiException exception, [CallerMemberName] string functionName = "")
    {
        switch (exception.StatusCode)
        {
            case HttpStatusCode.NotFound:
                Console.WriteLine($"[{functionName}] Resource not found.");
                break;
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.MethodNotAllowed:
                Console.WriteLine($"[{functionName}] Bad request.");
                break;
            default:
                Console.WriteLine($"[{functionName}] An error occurred: {exception.Message}");
                break;
        }
    }
}