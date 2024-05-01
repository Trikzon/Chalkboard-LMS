using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class EnrollStudentsPage : ContentPage
{
    public EnrollStudentsPage(Guid courseId)
    {
        InitializeComponent();
        BindingContext = new EnrollStudentsPageViewModel(courseId);
    }

    private async void EnrollStudents_OnPressed(object? sender, EventArgs e)
    {
        await ((EnrollStudentsPageViewModel)BindingContext).EnrollStudents();
        
        await Shell.Current.Navigation.PopAsync();
    }
}