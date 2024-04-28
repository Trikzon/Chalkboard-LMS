namespace Lms.BackEnd.WebApi.Contracts;

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