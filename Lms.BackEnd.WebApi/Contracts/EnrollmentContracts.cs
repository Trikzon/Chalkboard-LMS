namespace Lms.BackEnd.WebApi.Contracts;

public record CreateEnrollmentRequest(
    Guid CourseId,
    Guid StudentId
);

public record EnrollmentResponse(
    Guid CourseId,
    Guid StudentId
);