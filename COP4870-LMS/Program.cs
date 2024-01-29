using COP4870_LMS.CLI;
using COP4870_LMS.Helpers;
using COP4870_LMS.Models;
using COP4870_LMS.Services;

namespace COP4870_LMS;

internal static class Program
{
    public static void Main(string[] args)
    {
        var courseService = new CourseService(new List<Course>
        {
            new() { Code = "COP4580", Name = "Full-stack App Dev in C#", Description = "This is a description." }
        });
        var personService = new PersonService(new List<Person>
        {
            new() { Name = "Alice", Classification = Classification.Junior, Grades = 2.84 },
            new() { Name = "Bob", Classification = Classification.Senior, Grades = 2.84 },
        });
        var assignmentService = new AssignmentService();
        var moduleService = new ModuleService();
        var contentItemService = new ContentItemService();

        var personHelper = new PersonHelper(personService, courseService);
        var courseHelper = new CourseHelper(personService, courseService);
        var assignmentHelper = new AssignmentHelper(assignmentService, courseService);
        var moduleHelper = new ModuleHelper(moduleService, courseService);
        var contentItemHelper = new ContentItemHelper(contentItemService, moduleService);

        var cliProgram = new CliProgram();
        
        cliProgram.NavigateTo(() => new Menu("LMS", goBack: false)
            .AddOption("Create Course", courseHelper.CreateCourse)
            .AddOption("List Courses", courseHelper.ListCourses)
            .AddOption("Search Courses", courseHelper.SearchCourses)
            .AddOption("Select Course", courseHelper.SelectCourse, courseId =>
            {
                var course = courseService.GetCourse(courseId) ?? new Course { Name = "Invalid" };
                return new Menu($"Course ({course.Code})")
                    .AddInfo($"Code: {course.Code}")
                    .AddInfo($"Name: {course.Name}")
                    .AddInfo($"Description: {course.Description}")
                    .AddOption("Update Code", _ => courseHelper.UpdateCode(course))
                    .AddOption("Update Name", _ => courseHelper.UpdateName(course))
                    .AddOption("Update Description", _ => courseHelper.UpdateDescription(course))
                    .AddOption("Delete", cli => courseHelper.DeleteCourse(cli, course))
                    .AddOption("Enroll Student", cli => courseHelper.EnrollStudent(cli, course))
                    .AddOption("Drop Student", _ => courseHelper.DropStudent(course))
                    .AddOption("List Enrolled Students", _ => courseHelper.ListEnrolledStudents(course))
                    .AddOption("Create Assignment", _ => assignmentHelper.CreateAssignment(course))
                    .AddOption("List Assignments", _ => assignmentHelper.ListAssignments(course))
                    .AddOption("Select Assignment", _ => assignmentHelper.SelectAssignment(course), assignmentId =>
                    {
                        var assignment = assignmentService.GetAssignment(assignmentId) ?? new Assignment { Name = "Invalid" };
                        return new Menu($"Assignment ({assignment.Name})")
                            .AddInfo($"Name: {assignment.Name}")
                            .AddInfo($"Description: {assignment.Description}")
                            .AddInfo($"Total Available Points: {assignment.TotalAvailablePoints}")
                            .AddInfo($"Due Date: {assignment.DueDate.ToShortDateString()}")
                            .AddOption("Update Name", _ => assignmentHelper.UpdateName(assignment))
                            .AddOption("Update Description", _ => assignmentHelper.UpdateDescription(assignment))
                            .AddOption("Update Total Available Points", _ => assignmentHelper.UpdateTotalAvailablePoints(assignment))
                            .AddOption("Update Due Date", _ => assignmentHelper.UpdateDueDate(assignment))
                            .AddOption("Delete", cli => assignmentHelper.DeleteAssignment(cli, assignment));
                    })
                    .AddOption("Create Module", _ => moduleHelper.CreateModule(course))
                    .AddOption("List Modules", _ => moduleHelper.ListModules(course))
                    .AddOption("Select Modules", _ => moduleHelper.SelectModule(course), moduleId =>
                    {
                        var module = moduleService.GetModule(moduleId) ?? new Module { Name = "Invalid" };
                        return new Menu($"Module ({module.Name})")
                            .AddInfo($"Name: {module.Name}")
                            .AddInfo($"Description: {module.Description}")
                            .AddOption("Update Name", _ => moduleHelper.UpdateName(module))
                            .AddOption("Update Description", _ => moduleHelper.UpdateDescription(module))
                            .AddOption("Delete", cli => moduleHelper.DeleteModule(cli, module))
                            .AddOption("Create Content Item", _ => contentItemHelper.CreateContentItem(module))
                            .AddOption("List Content Items", _ => contentItemHelper.ListContentItems(module))
                            .AddOption("Select Content", _ => contentItemHelper.SelectContentItem(module), contentItemId =>
                            {
                                var contentItem = contentItemService.GetContentItem(contentItemId) ?? new ContentItem { Name = "Invalid" };
                                return new Menu($"Content Item ({contentItem.Name})")
                                    .AddInfo($"Name: {contentItem.Name}")
                                    .AddInfo($"Description: {contentItem.Description}")
                                    .AddInfo($"Path: {contentItem.Path}")
                                    .AddOption("Update Name", _ => contentItemHelper.UpdateName(contentItem))
                                    .AddOption("Update Description", _ => contentItemHelper.UpdateDescription(contentItem))
                                    .AddOption("Update Path", _ => contentItemHelper.UpdatePath(contentItem))
                                    .AddOption("Delete", cli => contentItemHelper.DeleteContentItem(cli, contentItem));
                            });
                    });
            })
            .AddOption("Create Student", personHelper.CreateStudent)
            .AddOption("List Students", personHelper.ListStudents)
            .AddOption("Search Students", personHelper.SearchStudents)
            .AddOption("Select Student", personHelper.SelectStudent, personId =>
            {
                var person = personService.GetPerson(personId) ?? new Person { Name = "Invalid" };
                return new Menu($"Student ({person.Name})")
                    .AddInfo($"Name: {person.Name}")
                    .AddInfo($"Classification: {person.Classification}")
                    .AddInfo($"Grades: {person.Grades}")
                    .AddOption("Update Name", _ => personHelper.UpdateName(person))
                    .AddOption("Update Classification", _ => personHelper.UpdateClassification(person))
                    .AddOption("Update Grades", _ => personHelper.UpdateGrades(person))
                    .AddOption("Delete", cli => personHelper.DeletePerson(cli, person))
                    .AddOption("List Enrolled Courses", _ => personHelper.ListEnrolledCourses(personId));
            })
            .AddOption("Exit", cli => cli.NavigateBack())
        );

        while (cliProgram.CanDisplay)
        {
            cliProgram.Display();
        }
    }
}