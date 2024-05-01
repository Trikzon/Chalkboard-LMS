using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class InstructorPageViewModel : INotifyPropertyChanged
{
    private string _courseSearchQuery = "";
    private IEnumerable<Student>? _students;
    private string _studentSearchQuery = "";
    private IEnumerable<Course>? _courses;

    public IEnumerable<Course>? Courses
    {
        get => _courses?.Where(c => c.Name.Contains(CourseSearchQuery, StringComparison.OrdinalIgnoreCase));
        private set => _courses = value;
    }

    public IEnumerable<Student>? Students
    {
        get => _students?.Where(s => s.Name.Contains(StudentSearchQuery, StringComparison.OrdinalIgnoreCase));
        private set => _students = value;
    }

    public string CourseSearchQuery
    {
        get => _courseSearchQuery;
        set
        {
            _courseSearchQuery = value;
            OnPropertyChanged(nameof(Courses));
        }
    }

    public string StudentSearchQuery
    {
        get => _studentSearchQuery;
        set
        {
            _studentSearchQuery = value;
            OnPropertyChanged(nameof(Students));
        }
    }

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
    
    public async Task<Course?> CreateCourseAsync()
    {
        var course = await CourseService.Current.CreateCourseAsync("New Course", "NEW");
        
        await UpdateAsync();
        
        return course;
    }
    
    public async Task DeleteCourseAsync(Course course)
    {
        await CourseService.Current.DeleteCourseAsync(course.Id);
        await UpdateAsync();
    }

    public async Task<Student?> CreateStudentAsync()
    {
        var student = await StudentService.Current.CreateStudentAsync("New Student", Classification.Freshman);
        
        await UpdateAsync();
        
        return student;
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