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
            initialValue: ((CourseViewModel)BindingContext).Course.Name
        );
        if (result != null)
        {
            ((CourseViewModel)BindingContext).UpdateName(result);
        }
    }
    
    private async void EditCourseCode_OnClicked(object? sender, EventArgs e)
    {
        var result = await DisplayPromptAsync(
            "Edit Course Code", "", "Save",
            placeholder: "Course Code",
            initialValue: ((CourseViewModel)BindingContext).Course.Code
        );
        if (result != null)
        {
            ((CourseViewModel)BindingContext).UpdateCode(result);
        }
    }
    
    private async void EditCourseDescription_OnClicked(object? sender, EventArgs e)
    {
        var result = await DisplayPromptAsync(
            "Edit Course Description", "", "Save",
            placeholder: "Course Description",
            initialValue: ((CourseViewModel)BindingContext).Course.Description
        );
        if (result != null)
        {
            ((CourseViewModel)BindingContext).UpdateDescription(result);
        }
    }
}