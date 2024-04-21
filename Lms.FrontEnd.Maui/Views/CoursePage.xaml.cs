using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class CoursePage : ContentPage
{
    public CoursePage(CourseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void EditCourseName_OnClicked(object? sender, EventArgs e)
    {
        var result = await DisplayPromptAsync(
            "Edit Course Name", "", "Save",
            placeholder: "Course Name",
            initialValue: ((CourseViewModel)BindingContext).Name
        );
        if (result != null)
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
        if (result != null)
        {
            ((CourseViewModel)BindingContext).Code = result;
        }
    }
}