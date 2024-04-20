using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        
        CourseService.Current.AddCourse(new Course{Code = "COP4530", Name = "Data Structures II", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."});
        CourseService.Current.AddCourse(new Course{Code = "GER2220", Name = "German 3", Description = "Sie ist eine deutsche Klasse."});
    }
}