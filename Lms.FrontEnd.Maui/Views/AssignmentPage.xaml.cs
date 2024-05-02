using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class AssignmentPage : ContentPage
{
    public AssignmentPage(Guid contentItemId, Guid moduleId, Guid? studentId = null)
    {
        InitializeComponent();
        BindingContext = new AssignmentPageViewModel(contentItemId, moduleId, studentId);
    }
    
    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await ((AssignmentPageViewModel)BindingContext).SaveChangesAsync();
    }
}