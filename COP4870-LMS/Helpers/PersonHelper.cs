using COP4870_LMS.CLI;
using COP4870_LMS.Models;
using COP4870_LMS.Services;

namespace COP4870_LMS.Helpers;

public class PersonHelper
{
    private readonly PersonService _personService;
    private readonly CourseService _courseService;
    
    public PersonHelper(PersonService personService, CourseService courseService)
    {
        _personService = personService;
        _courseService = courseService;
    }
    
    public void CreateStudent()
    {
        var student = new Person();
        
        if (!UpdateName(student))
            return;
        
        if (!UpdateClassification(student))
            return;
        
        if (!UpdateGrades(student))
            return;
        
        _personService.AddPerson(student);
        Console.WriteLine($"Successfully Created Student {student}.");
    }

    public void ListStudents()
    {
        Utils.DisplayList(_personService.GetList());
    }

    public void SearchStudents(CliProgram cli)
    {
        var results = _personService.Search(Utils.ReadString("Search Query"));
        cli.DisplayHeader("Search Students");
        Utils.DisplayList(results);
    }

    public Guid? SelectStudent(CliProgram cli)
    {
        var results = _personService.Search(Utils.ReadString("Search Query"));
        
        cli.DisplayHeader("Select Student");
        if (!Utils.TrySelectFromList("Student", results, out var student) || student == null)
        {
            return null;
        }

        return student.Id;
    }

    public bool UpdateName(Person person)
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

    public bool UpdateClassification(Person person)
    {
        if (!person.TryParseClassification(Utils.ReadString("Classification")))
        {
            Console.WriteLine("Invalid input. Valid Input is \"freshman\", \"sophomore\", etc.");
            return false;
        }

        return true;
    }

    public bool UpdateGrades(Person person)
    {
        if (!double.TryParse(Utils.ReadString("Grades (GPA)"), out var grades))
        {
            Console.WriteLine("Invalid Input. Please Enter a Decimal Value.");
            return false;
        }

        person.Grades = grades;
        return true;
    }
    
    public void DeletePerson(CliProgram cli, Person person)
    {
        if (Utils.ConfirmDeletion("Person"))
        {
            foreach (var course in _courseService.FilterByPerson(person.Id))
            {
                course.Roster.Remove(person.Id);
            }
            _personService.RemovePerson(person);
            Console.WriteLine("Successfully Deleted the Person.");
            cli.NavigateBack();
        }
    }

    public void ListEnrolledCourses(Guid id)
    {
        Utils.DisplayList(_courseService.FilterByPerson(id));
    }
}