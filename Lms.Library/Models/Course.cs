namespace Lms.Library.Models;

public class Course
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Guid> Roster { get; } = [];
    public List<Guid> Assignments { get; } = [];
    public List<Guid> Modules { get; } = [];

    public override string ToString()
    {
        return $"({Code}) {Name}";
    }
}