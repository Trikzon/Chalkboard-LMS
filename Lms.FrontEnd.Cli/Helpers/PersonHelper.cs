using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public static class PersonHelper
{
    public static void CreateStudent()
    {
        var student = new Person();
        
        if (!UpdateName(student))
            return;
        
        if (!UpdateClassification(student))
            return;
        
        if (!UpdateGrades(student))
            return;
        
        PersonService.Current.AddPerson(student);
        Console.WriteLine($"Successfully Created Student {student}.");
    }

    public static void ListStudents()
    {
        Utils.DisplayList(PersonService.Current.GetList());
    }

    public static void SearchStudents(CliProgram cli)
    {
        var results = PersonService.Current.Search(Utils.ReadString("Search Query"));
        cli.DisplayHeader("Search Students");
        Utils.DisplayList(results);
    }

    public static Guid? SelectStudent(CliProgram cli)
    {
        var results = PersonService.Current.Search(Utils.ReadString("Search Query"));
        
        cli.DisplayHeader("Select Student");
        if (!Utils.TrySelectFromList("Student", results, out var student) || student == null)
        {
            return null;
        }

        return student.Id;
    }

    public static bool UpdateName(Person person)
    {
        var name = Utils.ReadString("Name");
        if (name == "")
        {
            Console.WriteLine("Invalid input.");
            return false;
        }
        
        person.Name = name;
        return true;
    }

    public static bool UpdateClassification(Person person)
    {
        if (!person.TryParseClassification(Utils.ReadString("Classification")))
        {
            Console.WriteLine("Invalid input. Valid Input is \"freshman\", \"sophomore\", etc.");
            return false;
        }

        return true;
    }

    public static bool UpdateGrades(Person person)
    {
        if (!double.TryParse(Utils.ReadString("Grades (GPA)"), out var grades))
        {
            Console.WriteLine("Invalid Input. Please Enter a Decimal Value.");
            return false;
        }

        person.Grades = grades;
        return true;
    }
    
    public static void DeletePerson(CliProgram cli, Person person)
    {
        if (!Utils.ConfirmDeletion("Person")) return;
        
        foreach (var course in CourseService.Current.FilterByPerson(person.Id))
        {
            course.Roster.Remove(person.Id);
        }
        PersonService.Current.RemovePerson(person);
        Console.WriteLine("Successfully Deleted the Person.");
        cli.NavigateBack();
    }

    public static void ListEnrolledCourses(Guid id)
    {
        Utils.DisplayList(CourseService.Current.FilterByPerson(id));
    }
}