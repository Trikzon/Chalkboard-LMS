namespace Lms.Library.Contracts;

public record CreateEnrollmentRequest(
    Guid CourseId,
    Guid StudentId
);