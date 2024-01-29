namespace COP4870_LMS.Models;

public class Module
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid CourseId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<Guid> Content { get; } = new();
    
    public override string ToString()
    {
        return $"{Name}";
    }
}