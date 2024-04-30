using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class InstructorPage : ContentPage
{
    public ICommand EditCourseCommand { get; }
    public ICommand DeleteCourseCommand { get; }
    
    public InstructorPage()
    {
        EditCourseCommand = new Command<Course>(EditCourse);
        DeleteCourseCommand = new Command<Course>(DeleteCourse);
        
        InitializeComponent();
        BindingContext = new InstructorPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((InstructorPageViewModel)BindingContext).Update();
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
            await ((InstructorPageViewModel)BindingContext).DeleteCourse(course);
        }
    }

    private async void AddCourse_OnClicked(object? sender, EventArgs e)
    {
        await ((InstructorPageViewModel)BindingContext).CreateCourse();
    }
}