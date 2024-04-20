namespace Lms.FrontEnd.Maui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void Instructor_ButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new InstructorPage());
    }

    private async void Student_ButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}