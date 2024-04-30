using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.WebApi;

public interface ICoursesApi
{
    [Post("/courses")]
    Task<Course> CreateCourseAsync(CreateCourseRequest request);
    
    [Get("/courses")]
    Task<IEnumerable<Course>> GetCoursesAsync();
    
    [Get("/courses/{courseId}")]
    Task<Course?> GetCourseAsync(Guid courseId);
    
    [Put("/courses/{courseId}")]
    Task UpdateCourseAsync(Guid courseId, UpdateCourseRequest request);
    
    [Delete("/courses/{courseId}")]
    Task DeleteCourseAsync(Guid courseId);
}