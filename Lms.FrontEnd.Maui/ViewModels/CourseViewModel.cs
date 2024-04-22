using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CourseViewModel(Course course, Person? student) : INotifyPropertyChanged
{
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
    
    public IReadOnlyList<Person> Roster => course.Roster
        .Select(PersonService.Current.GetPerson)
        .Where(person => person is not null)
        .Select(person => person!)
        .ToList();
    
    public bool IsInstructor => student is null;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}