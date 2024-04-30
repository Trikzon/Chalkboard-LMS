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
        Task.Run(Update);
    }

    public async Task Update()
    {
        Courses = await CourseService.Current.GetCoursesAsync();
        OnPropertyChanged(nameof(Courses));
        Students = await StudentService.Current.GetStudentsAsync();
        OnPropertyChanged(nameof(Students));
    }
    
    public async Task CreateCourse()
    {
        await CourseService.Current.CreateCourseAsync("New Course", "NEW");
        await Update();
    }
    
    public async Task DeleteCourse(Course course)
    {
        await CourseService.Current.DeleteCourseAsync(course.Id);
        await Update();
    }

    public async Task CreateStudent()
    {
        await StudentService.Current.CreateStudentAsync("New Student", Classification.Freshman);
        await Update();
    }
    
    public async Task DeleteStudent(Student student)
    {
        await StudentService.Current.DeleteStudentAsync(student.Id);
        await Update();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}