using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class StudentPageViewModel : INotifyPropertyChanged
{
    private IEnumerable<Course>? _enrolledCourses;
    private string _searchQuery = "";

    public Guid StudentId { get; }

    public string Name { get; private set; } = "";

    public IEnumerable<Course>? EnrolledCourses
    {
        get => _enrolledCourses?.Where(s => s.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        private set => _enrolledCourses = value;
    }

    public Course? SelectedCourse { get; set; }
    
    public bool IsCourseSelected => SelectedCourse != null;
    
    public ICommand SelectionChangedCommand => new Command(() => OnPropertyChanged(nameof(IsCourseSelected)));
    
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            SelectedCourse = null;
            OnPropertyChanged(nameof(EnrolledCourses));
            OnPropertyChanged(nameof(IsCourseSelected));
        }
    }
    
    public StudentPageViewModel(Guid studentId)
    {
        StudentId = studentId;
        
        Task.Run(async () =>
        {
            var student = await StudentService.Current.GetStudentAsync(StudentId);
            if (student is not null)
            {
                Name = student.Name;
                OnPropertyChanged(nameof(Name));
            }
        });
    }

    public async Task UpdateAsync()
    {
        EnrolledCourses = await StudentService.Current.GetEnrolledCoursesAsync(StudentId);
        OnPropertyChanged(nameof(EnrolledCourses));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}