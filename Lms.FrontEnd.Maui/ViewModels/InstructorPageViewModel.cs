using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.FrontEnd.Maui.Views;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

internal sealed class InstructorPageViewModel : INotifyPropertyChanged
{
    public IReadOnlyList<Course> Courses { get; private set; }
    public IReadOnlyList<Person> Students { get; private set; }

    public InstructorPageViewModel()
    {
        Update();
    }
    
    public void Update()
    {
        Courses = CourseService.Current.GetList();
        OnPropertyChanged(nameof(Courses));
        Students = PersonService.Current.GetList();
        OnPropertyChanged(nameof(Students));
    }
    
    public void AddCourse(Course course)
    {
        CourseService.Current.AddCourse(course);
        Update();
    }

    public void DeleteCourse(Course course)
    {
        CourseService.Current.RemoveCourse(course);
        Update();
    }
    
    public void AddStudent(Person student)
    {
        PersonService.Current.AddPerson(student);
        Update();
    }
    
    public void DeleteStudent(Person student)
    {
        PersonService.Current.RemovePerson(student);
        Update();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}