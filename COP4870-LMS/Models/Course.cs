namespace COP4870_LMS.Models;

public class Course
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Guid> Roster { get; } = new();
    public List<Guid> Assignments { get; } = new();
    public List<Guid> Modules { get; } = new();

    public override string ToString()
    {
        return $"({Code}) {Name}";
    }
}