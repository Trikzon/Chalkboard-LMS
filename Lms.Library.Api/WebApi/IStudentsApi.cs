using Lms.Library.Contracts;
using Lms.Library.Models;
using Refit;

namespace Lms.Library.Api.WebApi;

public interface IStudentsApi
{
    [Post("/students")]
    Task<Student> CreateStudentAsync(CreateStudentRequest request);
    
    [Get("/students")]
    Task<IEnumerable<Student>> GetStudentsAsync();
    
    [Get("/students/{studentId}")]
    Task<Student?> GetStudentAsync(Guid studentId);
    
    [Put("/students/{studentId}")]
    Task UpdateStudentAsync(Guid studentId, UpdateStudentRequest request);
    
    [Delete("/students/{studentId}")]
    Task DeleteStudentAsync(Guid studentId);
}