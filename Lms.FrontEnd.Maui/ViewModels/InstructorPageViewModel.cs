using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

internal class InstructorPageViewModel
{
    public List<CourseViewModel> Courses { get; }
    
    public InstructorPageViewModel()
    {
        Courses = CourseService.Current
            .GetList()
            .Select(course => new CourseViewModel(course, null))
            .ToList();
    }
}