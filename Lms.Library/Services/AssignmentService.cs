using Lms.Library.Databases;
using Lms.Library.Models;

namespace Lms.Library.Services;

public class AssignmentService
{
    private static AssignmentService? _instance;
    public static AssignmentService Current => _instance ??= new AssignmentService();
    
    private readonly List<Assignment> _assignments = FakeDatabase.Assignments;
    
    private AssignmentService() { }

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