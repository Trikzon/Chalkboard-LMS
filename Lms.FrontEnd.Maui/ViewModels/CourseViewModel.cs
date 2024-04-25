using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CourseViewModel(Course course, Person? student=null) : INotifyPropertyChanged
{
    public Course Course => course;
    
    public string Name
    {
        get => course.Name;
        set
        {
            course.Name = value;
            OnPropertyChanged();
        }
    }
    
    public string Code
    {
        get => course.Code;
        set
        {
            course.Code = value;
            OnPropertyChanged();
        }
    }
    
    public string Description
    {
        get => course.Description;
        set
        {
            course.Description = value;
            OnPropertyChanged();
        }
    }
    
    public RosterViewModel Roster => new RosterViewModel(course);
    
    public bool IsInstructor => student is null;
    
    public ICommand RemoveStudentCommand => new Command<Person>(RemoveStudent);
    
    public void Update()
    {
        Roster.Update();
        OnPropertyChanged(nameof(Roster));
    }
    
    private void RemoveStudent(Person student)
    {
        Roster.Unenroll(student);
        OnPropertyChanged(nameof(Roster));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}