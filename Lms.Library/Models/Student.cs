namespace Lms.Library.Models;

public record Student(
    Guid Id,
    string Name,
    Classification Classification
);