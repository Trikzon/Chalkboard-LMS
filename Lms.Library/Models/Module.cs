namespace Lms.Library.Models;

public record Module(
    Guid Id,
    Guid CourseId,
    string Name
);