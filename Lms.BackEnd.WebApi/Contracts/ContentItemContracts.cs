namespace Lms.BackEnd.WebApi.Contracts;

public record CreateContentItemRequest(
    string Name,
    string? Content
);

public record CreateAssignmentRequest(
    string Name,
    string? Content,
    double TotalAvailablePoints,
    DateTime DueDate
) : CreateContentItemRequest(Name, Content);

public record UpdateContentItemRequest(
    string Name,
    string? Content
);

public record UpdateAssignmentRequest(
    string Name,
    string? Content,
    double TotalAvailablePoints,
    DateTime DueDate
) : UpdateContentItemRequest(Name, Content);

public record ContentItemResponse(
    Guid Id,
    Guid ModuleId,
    string Name,
    string? Content
);

public record AssignmentResponse(
    Guid Id,
    Guid ModuleId,
    string Name,
    string? Content,
    double TotalAvailablePoints,
    DateTime DueDate
) : ContentItemResponse(Id, ModuleId, Name, Content);