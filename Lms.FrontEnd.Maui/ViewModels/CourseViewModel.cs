using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Models;

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

    public bool IsInstructor => student is null;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}