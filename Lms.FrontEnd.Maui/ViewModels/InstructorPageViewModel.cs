using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class InstructorPageViewModel : INotifyPropertyChanged
{
    public IEnumerable<Course>? Courses { get; private set; }
    public IEnumerable<Student>? Students { get; private set; }
    
    public InstructorPageViewModel()
    {
        Task.Run(UpdateAsync);
    }

    public async Task UpdateAsync()
    {
        Courses = await CourseService.Current.GetCoursesAsync();
        OnPropertyChanged(nameof(Courses));
        Students = await StudentService.Current.GetStudentsAsync();
        OnPropertyChanged(nameof(Students));
    }
    
    public async Task CreateCourseAsync()
    {
        await CourseService.Current.CreateCourseAsync("New Course", "NEW");
        await UpdateAsync();
    }
    
    public async Task DeleteCourseAsync(Course course)
    {
        await CourseService.Current.DeleteCourseAsync(course.Id);
        await UpdateAsync();
    }

    public async Task CreateStudentAsync()
    {
        await StudentService.Current.CreateStudentAsync("New Student", Classification.Freshman);
        await UpdateAsync();
    }
    
    public async Task DeleteStudentAsync(Student student)
    {
        await StudentService.Current.DeleteStudentAsync(student.Id);
        await UpdateAsync();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}