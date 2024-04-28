namespace Lms.BackEnd.WebApi.Contracts;

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

public record CourseResponse(
    Guid Id,
    string Name,
    string Code,
    string? Description
);