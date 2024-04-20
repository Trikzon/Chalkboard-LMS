using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CourseViewModel(Course course, Person? student) : INotifyPropertyChanged
{
    public Course Course { get; } = course;
    public Person? Student { get; } = student;
    
    public bool IsInstructor => Student is null;
    
    public void UpdateName(string name)
    {
        Course.Name = name;
        OnPropertyChanged(nameof(Course));
    }
    
    public void UpdateCode(string code)
    {
        Course.Code = code;
        OnPropertyChanged(nameof(Course));
    }
    
    public void UpdateDescription(string description)
    {
        Course.Description = description;
        OnPropertyChanged(nameof(Course));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}