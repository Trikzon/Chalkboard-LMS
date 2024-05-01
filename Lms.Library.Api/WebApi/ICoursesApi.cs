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
    
    [Post("/courses/{courseId}/enrollments/{studentId}")]
    Task CreateEnrollmentAsync(Guid courseId, Guid studentId);
    
    [Get("/courses/{courseId}/enrollments")]
    Task<IEnumerable<Student>> GetEnrolledStudentsAsync(Guid courseId);
    
    [Delete("/courses/{courseId}/enrollments/{studentId}")]
    Task DeleteEnrollmentAsync(Guid courseId, Guid studentId);
}