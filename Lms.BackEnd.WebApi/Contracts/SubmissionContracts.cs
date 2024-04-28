namespace Lms.BackEnd.WebApi.Contracts;

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

public record SubmissionResponse(
    Guid ContentItemId,
    Guid StudentId,
    string? Content,
    DateTime SubmissionDate,
    float Points
);