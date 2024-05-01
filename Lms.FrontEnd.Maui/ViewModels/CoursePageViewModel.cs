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

    public CoursePageViewModel(Guid courseId)
    {
        CourseId = courseId;
        
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

        Task.Run(Update);
    }
    
    public async Task Update()
    {
        Roster = await CourseService.Current.GetEnrolledStudentsAsync(CourseId);
        OnPropertyChanged(nameof(Roster));
    }
    
    public async Task Save()
    {
        await CourseService.Current.UpdateCourseAsync(CourseId, Name, Code, Description);
    }
    
    public async Task DeleteStudentEnrollment(Student student)
    {
        await CourseService.Current.DeleteEnrollmentAsync(CourseId, student.Id);
        await Update();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}