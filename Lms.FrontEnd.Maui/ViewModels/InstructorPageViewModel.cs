using System.Windows.Input;
using Lms.FrontEnd.Maui.Views;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

internal class InstructorPageViewModel
{
    public List<CourseViewModel> Courses { get; }
    public IReadOnlyList<Person> Students { get; }

    public ICommand EditCourseCommand { get; }
    
    public InstructorPageViewModel()
    {
        Courses = CourseService.Current
            .GetList()
            .Select(course => new CourseViewModel(course, null))
            .ToList();
        
        Students = PersonService.Current.GetList();
        
        EditCourseCommand = new Command<CourseViewModel>(OpenCoursePage);
    }
    
    private async void OpenCoursePage(CourseViewModel viewModel)
    {
        await Shell.Current.Navigation.PushAsync(new CoursePage(viewModel));
    }
}