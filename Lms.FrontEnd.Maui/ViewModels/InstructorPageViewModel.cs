using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Contracts;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class InstructorPageViewModel : INotifyPropertyChanged
{
    public IEnumerable<Course>? Courses { get; private set; }
    
    public InstructorPageViewModel()
    {
        Task.Run(Update);
    }

    public async Task Update()
    {
        Courses = await CourseService.Current.GetCoursesAsync();
        OnPropertyChanged(nameof(Courses));
    }
    
    public async Task CreateCourse()
    {
        await CourseService.Current.CreateCourseAsync(new CreateCourseRequest("New Course", "NEW", null));
        await Update();
    }
    
    public async Task DeleteCourse(Course course)
    {
        await CourseService.Current.DeleteCourseAsync(course.Id);
        await Update();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}