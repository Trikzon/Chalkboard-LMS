namespace Lms.Library.Contracts;

public record CreateSubmissionRequest(
    string? Content,
    DateTime SubmissionDate,
    float Points
);

public record UpdateSubmissionRequest(
    string? Content,
    DateTime SubmissionDate,
    float Points
);