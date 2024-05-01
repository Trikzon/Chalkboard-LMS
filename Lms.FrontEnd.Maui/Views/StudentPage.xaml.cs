using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class StudentPage : ContentPage
{
    public StudentPage(Guid studentId)
    {
        InitializeComponent();
        BindingContext = new StudentPageViewModel(studentId);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((StudentPageViewModel)BindingContext).UpdateAsync();
    }

    private async void OpenCourse_OnPressed(object? sender, EventArgs e)
    {
        if (BindingContext is StudentPageViewModel { IsCourseSelected: true } viewModel)
        {
            await Navigation.PushAsync(new CoursePage(viewModel.SelectedCourse!.Id, viewModel.StudentId));
        }
    }
}