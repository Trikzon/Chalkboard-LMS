namespace Lms.Library.Models;

public record ContentItem(
    Guid Id,
    Guid ModuleId,
    string Name,
    string? Content
);

public record Assignment(
    Guid Id,
    Guid ModuleId,
    string Name,
    string? Content,
    double TotalAvailablePoints,
    DateTime DueDate
) : ContentItem(Id, ModuleId, Name, Content);