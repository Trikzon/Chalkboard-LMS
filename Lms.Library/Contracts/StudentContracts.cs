using Lms.Library.Models;

namespace Lms.Library.Contracts;

public record CreateStudentRequest(
    string Name,
    Classification Classification
);

public record UpdateStudentRequest(
    string Name,
    Classification Classification
);

public record StudentResponse(
    Guid Id,
    string Name,
    Classification Classification
);