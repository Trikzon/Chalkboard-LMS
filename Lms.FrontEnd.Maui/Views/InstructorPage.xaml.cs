using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Maui.Views;

public partial class InstructorPage : ContentPage
{
    public ICommand EditCourseCommand { get; }
    public ICommand DeleteCourseCommand { get; }
    public ICommand DeleteStudentCommand { get; }
    
    public InstructorPage()
    {
        EditCourseCommand = new Command<Course>(OpenCoursePage);
        DeleteCourseCommand = new Command<Course>(DeleteCourse);
        DeleteStudentCommand = new Command<Person>(DeleteStudent);
        
        InitializeComponent();
        BindingContext = new InstructorPageViewModel();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ((InstructorPageViewModel)BindingContext).Update();
    }
    
    private async void OpenCoursePage(Course course)
    {
        await Shell.Current.Navigation.PushAsync(new CoursePage(course));
    }
    
    private async void DeleteCourse(Course course)
    {
        var result = await DisplayAlert(
            "Delete Course", 
            $"Are you sure you want to delete\n{course.Name}?\nThis action cannot be undone.", 
            "Yes", "No"
        );

        if (result)
        {
            ((InstructorPageViewModel)BindingContext).DeleteCourse(course);
        }
    }
    
    private async void DeleteStudent(Person student)
    {
        var result = await DisplayAlert(
            "Delete Student", 
            $"Are you sure you want to delete\n{student.Name}?\nThis action cannot be undone.", 
            "Yes", "No"
        );

        if (result)
        {
            ((InstructorPageViewModel)BindingContext).DeleteStudent(student);
        }
    }

    private void AddCourse_OnClicked(object? sender, EventArgs e)
    {
        var course = new Course { Name = "New Course", Code = "NEW" };
        ((InstructorPageViewModel)BindingContext).AddCourse(course);
        OpenCoursePage(course);
    }

    private void AddStudent_OnClicked(object? sender, EventArgs e)
    {
       var student = new Person { Name = "New Student" };
       ((InstructorPageViewModel)BindingContext).AddStudent(student);
    }
}