using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class StudentLoginPage : ContentPage
{
    public StudentLoginPage()
    {
        InitializeComponent();
        BindingContext = new StudentLoginPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((StudentLoginPageViewModel)BindingContext).UpdateAsync();
    }

    private async void Login_OnPressed(object? sender, EventArgs e)
    {
        if (BindingContext is StudentLoginPageViewModel { IsStudentSelected: true } viewModel)
        {
            await Navigation.PushAsync(new StudentPage(viewModel.SelectedStudent!.Id));
        }
    }
}