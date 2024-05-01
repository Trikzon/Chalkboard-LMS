namespace Lms.Library.Models;

public record Submission(
    Guid ContentItemId,
    Guid StudentId,
    string? Content,
    DateTime SubmissionDate,
    float Points
);