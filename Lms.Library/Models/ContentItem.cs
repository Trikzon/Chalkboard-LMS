namespace Lms.Library.Models;

public class ContentItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid ModuleId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Path { get; set; } = "";
    
    public override string ToString()
    {
        return $"{Name}";
    }
}