using Lms.Library.Models;

namespace Lms.Library.Services;

public class AssignmentService
{
    private readonly IList<Assignment> _assignments;

    public AssignmentService()
    {
        _assignments = new List<Assignment>();
    }

    public AssignmentService(IList<Assignment> assignments)
    {
        _assignments = assignments;
    }

    public void AddAssignment(Assignment assignment)
    {
        _assignments.Add(assignment);
    }

    public void RemoveAssignment(Assignment assignment)
    {
        _assignments.Remove(assignment);
    }

    public Assignment? GetAssignment(Guid id)
    {
        return _assignments.FirstOrDefault(assignment => assignment.Id == id);
    }
}