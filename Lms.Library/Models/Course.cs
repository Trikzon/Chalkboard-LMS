namespace Lms.Library.Models;

public class Course(Guid id, string name, string code, string? description)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Code { get; } = code;
    public string? Description { get; } = description;
}