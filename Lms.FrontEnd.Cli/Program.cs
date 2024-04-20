using Lms.FrontEnd.Cli.CLI;
using Lms.FrontEnd.Cli.Helpers;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli;

internal static class Program
{
    public static void Main(string[] args)
    {
        var cliProgram = new CliProgram();
        
        cliProgram.NavigateTo(() => new Menu("LMS", goBack: false)
            .AddOption("Create Course", CourseHelper.CreateCourse)
            .AddOption("List Courses", CourseHelper.ListCourses)
            .AddOption("Search Courses", CourseHelper.SearchCourses)
            .AddOption("Select Course", CourseHelper.SelectCourse, courseId =>
            {
                var course = CourseService.Current.GetCourse(courseId) ?? new Course { Name = "Invalid" };
                return new Menu($"Course ({course.Code})")
                    .AddInfo($"Code: {course.Code}")
                    .AddInfo($"Name: {course.Name}")
                    .AddInfo($"Description: {course.Description}")
                    .AddOption("Update Code", _ => CourseHelper.UpdateCode(course))
                    .AddOption("Update Name", _ => CourseHelper.UpdateName(course))
                    .AddOption("Update Description", _ => CourseHelper.UpdateDescription(course))
                    .AddOption("Delete", cli => CourseHelper.DeleteCourse(cli, course))
                    .AddOption("Enroll Student", cli => CourseHelper.EnrollStudent(cli, course))
                    .AddOption("Drop Student", _ => CourseHelper.DropStudent(course))
                    .AddOption("List Enrolled Students", _ => CourseHelper.ListEnrolledStudents(course))
                    .AddOption("Create Assignment", _ => AssignmentHelper.CreateAssignment(course))
                    .AddOption("List Assignments", _ => AssignmentHelper.ListAssignments(course))
                    .AddOption("Select Assignment", _ => AssignmentHelper.SelectAssignment(course), assignmentId =>
                    {
                        var assignment = AssignmentService.Current.GetAssignment(assignmentId) ?? new Assignment { Name = "Invalid" };
                        return new Menu($"Assignment ({assignment.Name})")
                            .AddInfo($"Name: {assignment.Name}")
                            .AddInfo($"Description: {assignment.Description}")
                            .AddInfo($"Total Available Points: {assignment.TotalAvailablePoints}")
                            .AddInfo($"Due Date: {assignment.DueDate.ToShortDateString()}")
                            .AddOption("Update Name", _ => AssignmentHelper.UpdateName(assignment))
                            .AddOption("Update Description", _ => AssignmentHelper.UpdateDescription(assignment))
                            .AddOption("Update Total Available Points", _ => AssignmentHelper.UpdateTotalAvailablePoints(assignment))
                            .AddOption("Update Due Date", _ => AssignmentHelper.UpdateDueDate(assignment))
                            .AddOption("Delete", cli => AssignmentHelper.DeleteAssignment(cli, assignment));
                    })
                    .AddOption("Create Module", _ => ModuleHelper.CreateModule(course))
                    .AddOption("List Modules", _ => ModuleHelper.ListModules(course))
                    .AddOption("Select Modules", _ => ModuleHelper.SelectModule(course), moduleId =>
                    {
                        var module = ModuleService.Current.GetModule(moduleId) ?? new Module { Name = "Invalid" };
                        return new Menu($"Module ({module.Name})")
                            .AddInfo($"Name: {module.Name}")
                            .AddInfo($"Description: {module.Description}")
                            .AddOption("Update Name", _ => ModuleHelper.UpdateName(module))
                            .AddOption("Update Description", _ => ModuleHelper.UpdateDescription(module))
                            .AddOption("Delete", cli => ModuleHelper.DeleteModule(cli, module))
                            .AddOption("Create Content Item", _ => ContentItemHelper.CreateContentItem(module))
                            .AddOption("List Content Items", _ => ContentItemHelper.ListContentItems(module))
                            .AddOption("Select Content", _ => ContentItemHelper.SelectContentItem(module), contentItemId =>
                            {
                                var contentItem = ContentItemService.Current.GetContentItem(contentItemId) ?? new ContentItem { Name = "Invalid" };
                                return new Menu($"Content Item ({contentItem.Name})")
                                    .AddInfo($"Name: {contentItem.Name}")
                                    .AddInfo($"Description: {contentItem.Description}")
                                    .AddInfo($"Path: {contentItem.Path}")
                                    .AddOption("Update Name", _ => ContentItemHelper.UpdateName(contentItem))
                                    .AddOption("Update Description", _ => ContentItemHelper.UpdateDescription(contentItem))
                                    .AddOption("Update Path", _ => ContentItemHelper.UpdatePath(contentItem))
                                    .AddOption("Delete", cli => ContentItemHelper.DeleteContentItem(cli, contentItem));
                            });
                    });
            })
            .AddOption("Create Student", PersonHelper.CreateStudent)
            .AddOption("List Students", PersonHelper.ListStudents)
            .AddOption("Search Students", PersonHelper.SearchStudents)
            .AddOption("Select Student", PersonHelper.SelectStudent, personId =>
            {
                var person = PersonService.Current.GetPerson(personId) ?? new Person { Name = "Invalid" };
                return new Menu($"Student ({person.Name})")
                    .AddInfo($"Name: {person.Name}")
                    .AddInfo($"Classification: {person.Classification}")
                    .AddInfo($"Grades: {person.Grades}")
                    .AddOption("Update Name", _ => PersonHelper.UpdateName(person))
                    .AddOption("Update Classification", _ => PersonHelper.UpdateClassification(person))
                    .AddOption("Update Grades", _ => PersonHelper.UpdateGrades(person))
                    .AddOption("Delete", cli => PersonHelper.DeletePerson(cli, person))
                    .AddOption("List Enrolled Courses", _ => PersonHelper.ListEnrolledCourses(personId));
            })
            .AddOption("Exit", cli => cli.NavigateBack())
        );

        while (cliProgram.CanDisplay)
        {
            cliProgram.Display();
        }
    }
}