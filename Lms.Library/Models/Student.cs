namespace Lms.Library.Models;

public class Student(Guid id, string name, Classification classification)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public Classification Classification { get; } = classification;
}