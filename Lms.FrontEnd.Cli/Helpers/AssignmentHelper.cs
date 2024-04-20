using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public static class AssignmentHelper
{
    public static void CreateAssignment(Course course)
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
        AssignmentService.Current.AddAssignment(assignment);
        Console.WriteLine($"Successfully Created Assignment {assignment}.");
    }

    public static void ListAssignments(Course course)
    {
        Utils.DisplayList(course.Assignments.Select(id => AssignmentService.Current.GetAssignment(id)).ToList());
    }

    public static Guid? SelectAssignment(Course course)
    {
        var assignments = course.Assignments.Select(id => AssignmentService.Current.GetAssignment(id)).ToList();
        if (!Utils.TrySelectFromList("Assignment", assignments, out var assignment) || assignment == null)
        {
            return null;
        }

        return assignment.Id;
    }

    public static bool UpdateName(Assignment assignment)
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

    public static bool UpdateDescription(Assignment assignment)
    {
        assignment.Description = Utils.ReadString("Description");
        return true;
    }

    public static bool UpdateTotalAvailablePoints(Assignment assignment)
    {
        if (!double.TryParse(Utils.ReadString("Total Available Points"), out var totalAvailablePoints) && totalAvailablePoints >= 0)
        {
            Console.WriteLine("Invalid Input. Please Enter a Positive Decimal Value.");
            return false;
        }

        assignment.TotalAvailablePoints = totalAvailablePoints;
        return true;
    }

    public static bool UpdateDueDate(Assignment assignment)
    {
        if (!DateTime.TryParse(Utils.ReadString("Due Date"), out var dueDate))
        {
            Console.WriteLine("Invalid Input. Please Enter a Valid Date (e.g., 10/31/2024)");
            return false;
        }

        assignment.DueDate = dueDate;
        return true;
    }

    public static void DeleteAssignment(CliProgram cli, Assignment assignment)
    {
        if (!Utils.ConfirmDeletion("Assignment")) return;
        
        CourseService.Current.GetCourse(assignment.CourseId)?.Assignments.Remove(assignment.Id);
        AssignmentService.Current.RemoveAssignment(assignment);
        Console.WriteLine("Successfully Deleted the Assignment.");
        cli.NavigateBack();
    }
}