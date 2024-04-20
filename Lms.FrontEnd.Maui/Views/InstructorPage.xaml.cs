using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class InstructorPage : ContentPage
{
    public ICommand CourseCommand { get; }
    
    public InstructorPage()
    {
        CourseCommand = new Command<CourseViewModel>(OpenCoursePage);
        
        InitializeComponent();
        BindingContext = new InstructorPageViewModel();
    }

    private async void OpenCoursePage(CourseViewModel viewModel)
    {
        await Shell.Current.Navigation.PushAsync(new CoursePage(viewModel));
    }
}