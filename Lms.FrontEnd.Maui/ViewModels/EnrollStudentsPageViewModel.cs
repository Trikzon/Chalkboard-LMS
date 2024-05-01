using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class EnrollStudentsPageViewModel : INotifyPropertyChanged
{
    private readonly Guid _courseId;
    private IEnumerable<Student>? _notEnrolledStudents;
    
    public IEnumerable<Student>? NotEnrolledStudents => _notEnrolledStudents;

    public IList<Object> SelectedStudents { get; } = [];
    
    public bool HasSelectedStudents => SelectedStudents.Any();
    
    public ICommand SelectionChangedCommand => new Command(() => OnPropertyChanged(nameof(HasSelectedStudents)));

    public EnrollStudentsPageViewModel(Guid courseId)
    {
        _courseId = courseId;

        Task.Run(async () =>
        {
            var students = await StudentService.Current.GetStudentsAsync();
            var enrolledStudents = await CourseService.Current.GetEnrolledStudentsAsync(_courseId);
            
            if (enrolledStudents != null)
            {
                _notEnrolledStudents = students?.Except(enrolledStudents);
                OnPropertyChanged(nameof(NotEnrolledStudents));
            }
        });
    }
    
    public async Task EnrollStudents()
    {
        foreach (var student in SelectedStudents.Cast<Student>())
        {
            await CourseService.Current.CreateEnrollmentAsync(_courseId, student.Id);
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}