using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class InstructorPage : ContentPage
{
    public ICommand EditCourseCommand { get; }
    public ICommand DeleteCourseCommand { get; }
    public ICommand EditStudentCommand { get; }
    public ICommand DeleteStudentCommand { get; }
    
    public InstructorPage()
    {
        EditCourseCommand = new Command<Course>(EditCourse);
        DeleteCourseCommand = new Command<Course>(DeleteCourse);
        EditStudentCommand = new Command<Student>(EditStudent);
        DeleteStudentCommand = new Command<Student>(DeleteStudent);
        
        InitializeComponent();
        BindingContext = new InstructorPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((InstructorPageViewModel)BindingContext).UpdateAsync();
    }

    private async void AddCourse_OnClicked(object? sender, EventArgs e)
    {
        var course = await ((InstructorPageViewModel)BindingContext).CreateCourseAsync();
        
        if (course is not null)
        {
            await Shell.Current.Navigation.PushAsync(new CoursePage(course.Id));
        }
    }
    
    private async void EditCourse(Course course)
    {
        await Shell.Current.Navigation.PushAsync(new CoursePage(course.Id));
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
            await ((InstructorPageViewModel)BindingContext).DeleteCourseAsync(course);
        }
    }
    
    private async void AddStudent_OnClicked(object? sender, EventArgs e)
    {
        var student = await ((InstructorPageViewModel)BindingContext).CreateStudentAsync();
        
        if (student is not null)
        {
            await Shell.Current.Navigation.PushAsync(new EditStudentPage(student.Id));
        }
    }
    
    private async void EditStudent(Student student)
    {
        await Shell.Current.Navigation.PushAsync(new EditStudentPage(student.Id));
    }
    
    private async void DeleteStudent(Student student)
    {
        var result = await DisplayAlert(
            "Delete Student",
            $"Are you sure you want to delete\n{student.Name}?\nThis action cannot be undone.",
            "Yes", "No"
        );
        
        if (result)
        {
            await ((InstructorPageViewModel)BindingContext).DeleteStudentAsync(student);
        }
    }
}