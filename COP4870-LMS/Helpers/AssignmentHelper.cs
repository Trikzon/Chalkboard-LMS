using COP4870_LMS.CLI;
using COP4870_LMS.Models;
using COP4870_LMS.Services;

namespace COP4870_LMS.Helpers;

public class AssignmentHelper
{
    private readonly AssignmentService _assignmentService;
    private readonly CourseService _courseService;

    public AssignmentHelper(AssignmentService assignmentService, CourseService courseService)
    {
        _assignmentService = assignmentService;
        _courseService = courseService;
    }

    public void CreateAssignment(Course course)
    {
        var assignment = new Assignment();
        
        if (!UpdateName(assignment))
            return;
        
        if (!UpdateDescription(assignment))
            return;
        
        if (!UpdateTotalAvailablePoints(assignment))
            return;
        
        if (!UpdateDueDate(assignment))
            return;

        assignment.CourseId = course.Id;
        course.Assignments.Add(assignment.Id);
        _assignmentService.AddAssignment(assignment);
        Console.WriteLine($"Successfully Created Assignment {assignment}.");
    }

    public void ListAssignments(Course course)
    {
        Utils.DisplayList(course.Assignments.Select(id => _assignmentService.GetAssignment(id)).ToList());
    }

    public Guid? SelectAssignment(Course course)
    {
        var assignments = course.Assignments.Select(id => _assignmentService.GetAssignment(id)).ToList();
        if (!Utils.TrySelectFromList("Assignment", assignments, out var assignment) || assignment == null)
        {
            return null;
        }

        return assignment.Id;
    }

    public bool UpdateName(Assignment assignment)
    {
        var name = Utils.ReadString("Name");
        if (name == "")
        {
            Console.WriteLine("Invalid Input.");
            return false;
        }

        assignment.Name = name;
        return true;
    }

    public bool UpdateDescription(Assignment assignment)
    {
        assignment.Description = Utils.ReadString("Description");
        return true;
    }

    public bool UpdateTotalAvailablePoints(Assignment assignment)
    {
        if (!double.TryParse(Utils.ReadString("Total Available Points"), out var totalAvailablePoints) && totalAvailablePoints >= 0)
        {
            Console.WriteLine("Invalid Input. Please Enter a Positive Decimal Value.");
            return false;
        }

        assignment.TotalAvailablePoints = totalAvailablePoints;
        return true;
    }

    public bool UpdateDueDate(Assignment assignment)
    {
        if (!DateTime.TryParse(Utils.ReadString("Due Date"), out var dueDate))
        {
            Console.WriteLine("Invalid Input. Please Enter a Valid Date (e.g., 10/31/2024)");
            return false;
        }

        assignment.DueDate = dueDate;
        return true;
    }

    public void DeleteAssignment(CliProgram cli, Assignment assignment)
    {
        if (Utils.ConfirmDeletion("Assignment"))
        {
            _courseService.GetCourse(assignment.CourseId)?.Assignments.Remove(assignment.Id);
            _assignmentService.RemoveAssignment(assignment);
            Console.WriteLine("Successfully Deleted the Assignment.");
            cli.NavigateBack();
        }
    }
}