namespace Lms.FrontEnd.Maui.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void Instructor_ButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.Navigation.PushAsync(new InstructorPage());
    }

    private void Student_ButtonClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}