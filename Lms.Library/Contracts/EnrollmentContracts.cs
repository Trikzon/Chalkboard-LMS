namespace Lms.Library.Contracts;

public record CreateEnrollmentRequest(
    Guid CourseId,
    Guid StudentId
);

public record EnrollmentResponse(
    Guid CourseId,
    Guid StudentId
);