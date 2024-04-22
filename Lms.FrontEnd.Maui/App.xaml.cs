using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        
        var cop4530 = new Course { Code = "COP4530", Name = "Data Structures II", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." };
        var ger2220 = new Course { Code = "GER2220", Name = "German 3", Description = "Sie ist eine deutsche Klasse." };
        CourseService.Current.AddCourse(cop4530);
        CourseService.Current.AddCourse(ger2220);

        var bob = new Person { Name = "Bob" };
        var alice = new Person { Name = "Alice" };
        PersonService.Current.AddPerson(bob);
        PersonService.Current.AddPerson(alice);
        
        cop4530.Roster.Add(bob.Id);
        cop4530.Roster.Add(alice.Id);
        ger2220.Roster.Add(bob.Id);
    }
}