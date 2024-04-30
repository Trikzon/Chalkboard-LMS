namespace Lms.Library.Contracts;

public record CreateCourseRequest(
    string Name,
    string Code,
    string? Description
);

public record UpdateCourseRequest(
    string Name,
    string Code,
    string? Description
);