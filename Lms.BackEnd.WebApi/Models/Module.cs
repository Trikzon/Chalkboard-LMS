namespace Lms.BackEnd.WebApi.Models;

public class Module(Guid id, Guid courseId, string name)
{
    public Guid Id { get; } = id;
    public Guid CourseId { get; } = courseId;
    public string Name { get; } = name;
}