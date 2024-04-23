using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class RosterViewModel : INotifyPropertyChanged
{
    private readonly Course _course;
    private readonly bool _showEnrolled;
    
    private IReadOnlyList<Person> _students = Array.Empty<Person>();
    public IReadOnlyList<Person> Students
    {
        get => _students;
        private set
        {
            if (Equals(value, _students)) return;
            
            _students = value;
            OnPropertyChanged();
        }
    }

    public IList<Object> SelectedStudents { get; } = [];
    
    public bool HasSelectedStudents => SelectedStudents.Any();

    public ICommand SelectionChangedCommand { get; }
    
    public RosterViewModel(Course course, bool enrolled=true)
    {
        _course = course;
        _showEnrolled = enrolled;

        SelectionChangedCommand = new Command(() => OnPropertyChanged(nameof(HasSelectedStudents)));
        
        Update();
    }

    public void Update()
    {
        Students = PersonService.Current.GetList()
            .Where(person => _showEnrolled ? _course.Roster.Contains(person.Id) : !_course.Roster.Contains(person.Id))
            .ToList();
    }

    public void Enroll(Person student)
    {
        _course.Roster.Add(student.Id);
    }
    
    public void Unenroll(Person student)
    {
        _course.Roster.Remove(student.Id);
        Update();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}