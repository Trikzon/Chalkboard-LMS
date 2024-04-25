using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class CoursePage : ContentPage
{
    public CoursePage(Course course)
    {
        InitializeComponent();
        BindingContext = new CourseViewModel(course);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        ((CourseViewModel)BindingContext).Update();
    }

    private async void EditCourseName_OnClicked(object? sender, EventArgs e)
    {
        var result = await DisplayPromptAsync(
            "Edit Course Name", "", "Save",
            placeholder: "Course Name",
            initialValue: ((CourseViewModel)BindingContext).Name
        );
        if (!string.IsNullOrEmpty(result))
        {
            ((CourseViewModel)BindingContext).Name = result;
        }
    }
    
    private async void EditCourseCode_OnClicked(object? sender, EventArgs e)
    {
        var result = await DisplayPromptAsync(
            "Edit Course Code", "", "Save",
            placeholder: "Course Code",
            initialValue: ((CourseViewModel)BindingContext).Code
        );
        if (!string.IsNullOrEmpty(result))
        {
            ((CourseViewModel)BindingContext).Code = result;
        }
    }

    private async void AddStudents_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new AddStudentsPage(((CourseViewModel)BindingContext).Course));
    }
}