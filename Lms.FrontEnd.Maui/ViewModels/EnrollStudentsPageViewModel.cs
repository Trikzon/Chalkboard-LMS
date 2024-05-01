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
    private string _searchQuery = "";

    public IEnumerable<Student>? NotEnrolledStudents
    {
        get => _notEnrolledStudents?.Where(c => c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        private set => _notEnrolledStudents = value;
    }

    public IList<object> SelectedStudents { get; } = [];
    
    public bool HasSelectedStudents => SelectedStudents.Any();
    
    public ICommand SelectionChangedCommand => new Command(() => OnPropertyChanged(nameof(HasSelectedStudents)));

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            SelectedStudents.Clear();
            OnPropertyChanged(nameof(HasSelectedStudents));
            OnPropertyChanged(nameof(NotEnrolledStudents));
        }
    }

    public EnrollStudentsPageViewModel(Guid courseId)
    {
        _courseId = courseId;

        Task.Run(async () =>
        {
            var students = await StudentService.Current.GetStudentsAsync();
            var enrolledStudents = await CourseService.Current.GetEnrolledStudentsAsync(_courseId);
            
            if (enrolledStudents != null)
            {
                NotEnrolledStudents = students?.Except(enrolledStudents);
                OnPropertyChanged(nameof(NotEnrolledStudents));
            }
        });
    }
    
    public async Task EnrollStudentsAsync()
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