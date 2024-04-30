namespace Lms.Library.Contracts;

public record CreateModuleRequest(
    string Name
);

public record UpdateModuleRequest(
    string Name
);

public record ModuleResponse(
    Guid Id,
    Guid CourseId,
    string Name
);