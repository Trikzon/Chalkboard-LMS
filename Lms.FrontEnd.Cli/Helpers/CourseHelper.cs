using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public static class CourseHelper
{
    public static void CreateCourse()
    {
        var course = new Course();
        
        if (!UpdateCode(course))
            return;
        
        if (!UpdateName(course))
            return;
        
        if (!UpdateDescription(course))
            return;
        
        CourseService.Current.AddCourse(course);
        Console.WriteLine($"Successfully Created Course {course}.");
    }

    public static void ListCourses()
    {
        Utils.DisplayList(CourseService.Current.GetList());
    }

    public static void SearchCourses(CliProgram cli)
    {
        var results = CourseService.Current.Search(Utils.ReadString("Search Query"));
        cli.DisplayHeader("Search Students");
        Utils.DisplayList(results);
    }

    public static Guid? SelectCourse(CliProgram cli)
    {
        var results = CourseService.Current.Search(Utils.ReadString("Search Query"));
        
        cli.DisplayHeader("Select Course");
        if (!Utils.TrySelectFromList("Course", results, out var course) || course == null)
        {
            return null;
        }

        return course.Id;
    }

    public static bool UpdateCode(Course course)
    {
        var code = Utils.ReadString("Code");
        if (code == "")
        {
            Console.WriteLine("Invalid Input.");
            return false;
        }

        course.Code = code;
        return true;
    }

    public static bool UpdateName(Course course)
    {
        var name = Utils.ReadString("Name");
        if (name == "")
        {
            Console.WriteLine("Invalid Input.");
            return false;
        }

        course.Name = name;
        return true;
    }

    public static bool UpdateDescription(Course course)
    {
        course.Description = Utils.ReadString("Description");
        return true;
    }

    public static void DeleteCourse(CliProgram cli, Course course)
    {
        if (Utils.ConfirmDeletion("Course"))
        {
            CourseService.Current.RemoveCourse(course);
            Console.WriteLine("Successfully Deleted the Course.");
            cli.NavigateBack();
        }
    }

    public static void EnrollStudent(CliProgram cli, Course course)
    {
        var results = PersonService.Current.Search(Utils.ReadString("Search Query"))
            .Where(student => !course.Roster.Contains(student.Id))
            .ToList();
        
        cli.DisplayHeader("Enroll Student");
        
        if (!Utils.TrySelectFromList("Student", results, out var student) || student == null) return;
        
        course.Roster.Add(student.Id);
        Console.WriteLine($"Successfully Enrolled Student {student}");
    }

    public static void DropStudent(Course course)
    {
        var students = course.Roster.Select(id => PersonService.Current.GetPerson(id)).ToList();
        if (Utils.TrySelectFromList("Student", students, out var student) && student != null)
        {
            course.Roster.Remove(student.Id);
            Console.WriteLine($"Successfully Dropped Student {student}");
        }
    }

    public static void ListEnrolledStudents(Course course)
    {
        Utils.DisplayList(course.Roster.Select(id => PersonService.Current.GetPerson(id)).ToList());
    }
}