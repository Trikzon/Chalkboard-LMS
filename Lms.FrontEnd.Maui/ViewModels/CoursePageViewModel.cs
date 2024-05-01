using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CoursePageViewModel : INotifyPropertyChanged
{
    private string _name = "";
    private string _code = "";
    private string _description = "";
    
    public Guid CourseId { get; }
    
    public Guid? StudentId { get; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }
    
    public IEnumerable<Student>? Roster { get; private set; }
    
    public bool IsInstructor => StudentId is null;

    public CoursePageViewModel(Guid courseId, Guid? studentId)
    {
        CourseId = courseId;
        StudentId = studentId;
        
        Task.Run(async () =>
        {
            var course = await CourseService.Current.GetCourseAsync(CourseId);
            if (course is not null)
            {
                Name = course.Name;
                Code = course.Code;
                Description = course.Description ?? "";
            }
        });

        Task.Run(UpdateAsync);
    }
    
    public async Task UpdateAsync()
    {
        Roster = await CourseService.Current.GetEnrolledStudentsAsync(CourseId);
        OnPropertyChanged(nameof(Roster));
    }
    
    public async Task SaveChangesAsync()
    {
        await CourseService.Current.UpdateCourseAsync(CourseId, Name, Code, Description);
    }
    
    public async Task DeleteStudentEnrollmentAsync(Student student)
    {
        await CourseService.Current.DeleteEnrollmentAsync(CourseId, student.Id);
        await UpdateAsync();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}