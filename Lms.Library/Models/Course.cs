namespace Lms.Library.Models;

public record Course(
    Guid Id,
    string Name,
    string Code,
    string? Description
);