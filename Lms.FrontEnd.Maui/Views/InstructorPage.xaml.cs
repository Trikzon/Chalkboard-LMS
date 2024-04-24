using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class InstructorPage : ContentPage
{
    public InstructorPage()
    {
        InitializeComponent();
        BindingContext = new InstructorPageViewModel();
    }
}