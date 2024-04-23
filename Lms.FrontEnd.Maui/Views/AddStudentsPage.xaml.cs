using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class AddStudentsPage : ContentPage
{
    public AddStudentsPage(Course course)
    {
        InitializeComponent();
        BindingContext = new RosterViewModel(course, false);
    }

    private void AddStudents_OnPressed(object? sender, EventArgs e)
    {
        var viewModel = (RosterViewModel)BindingContext;
        foreach (var student in viewModel.SelectedStudents.Cast<Person>())
        {
            viewModel.Enroll(student);
        }
        Shell.Current.Navigation.PopAsync();
    }
}