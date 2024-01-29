using COP4870_LMS.CLI;
using COP4870_LMS.Models;
using COP4870_LMS.Services;

namespace COP4870_LMS.Helpers;

public class CourseHelper
{
    private readonly PersonService _personService;
    private readonly CourseService _courseService;

    public CourseHelper(PersonService personService, CourseService courseService)
    {
        _personService = personService;
        _courseService = courseService;
    }

    public void CreateCourse()
    {
        var course = new Course();
        
        if (!UpdateCode(course))
            return;
        
        if (!UpdateName(course))
            return;
        
        if (!UpdateDescription(course))
            return;
        
        _courseService.AddCourse(course);
        Console.WriteLine($"Successfully Created Course {course}.");
    }

    public void ListCourses()
    {
        Utils.DisplayList(_courseService.GetList());
    }

    public void SearchCourses(CliProgram cli)
    {
        var results = _courseService.Search(Utils.ReadString("Search Query"));
        cli.DisplayHeader("Search Students");
        Utils.DisplayList(results);
    }

    public Guid? SelectCourse(CliProgram cli)
    {
        var results = _courseService.Search(Utils.ReadString("Search Query"));
        
        cli.DisplayHeader("Select Course");
        if (!Utils.TrySelectFromList("Course", results, out var course) || course == null)
        {
            return null;
        }

        return course.Id;
    }

    public bool UpdateCode(Course course)
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

    public bool UpdateName(Course course)
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

    public bool UpdateDescription(Course course)
    {
        course.Description = Utils.ReadString("Description");
        return true;
    }

    public void DeleteCourse(CliProgram cli, Course course)
    {
        if (Utils.ConfirmDeletion("Course"))
        {
            _courseService.RemoveCourse(course);
            Console.WriteLine("Successfully Deleted the Course.");
            cli.NavigateBack();
        }
    }

    public void EnrollStudent(CliProgram cli, Course course)
    {
        var results = _personService.Search(Utils.ReadString("Search Query"))
                                    .Where(student => !course.Roster.Contains(student.Id))
                                    .ToList();
        
        cli.DisplayHeader("Enroll Student");
        if (Utils.TrySelectFromList("Student", results, out var student) && student != null)
        {
            course.Roster.Add(student.Id);
            Console.WriteLine($"Successfully Enrolled Student {student}");
        }
    }

    public void DropStudent(Course course)
    {
        var students = course.Roster.Select(id => _personService.GetPerson(id)).ToList();
        if (Utils.TrySelectFromList("Student", students, out var student) && student != null)
        {
            course.Roster.Remove(student.Id);
            Console.WriteLine($"Successfully Dropped Student {student}");
        }
    }

    public void ListEnrolledStudents(Course course)
    {
        Utils.DisplayList(course.Roster.Select(id => _personService.GetPerson(id)).ToList());
    }
}