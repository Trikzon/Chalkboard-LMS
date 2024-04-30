namespace Lms.Library.Models;

public class ContentItem(Guid id, Guid moduleId, string name, string? content)
{
    public Guid Id { get; } = id;
    public Guid ModuleId { get; } = moduleId;
    public string Name { get; } = name;
    public string? Content { get; } = content;
}

public class Assignment(Guid id, Guid moduleId, string name, string? content, double totalAvailablePoints, DateTime dueDate)
    : ContentItem(id, moduleId, name, content)
{
    public double TotalAvailablePoints { get; } = totalAvailablePoints;
    public DateTime DueDate { get; } = dueDate;
}