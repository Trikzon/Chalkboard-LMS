using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class GradeAssignmentPage : ContentPage
{
    public GradeAssignmentPage(SubmissionViewModel submissionViewModel)
    {
        InitializeComponent();
        BindingContext = submissionViewModel;
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await ((SubmissionViewModel)BindingContext).SaveChangesAsync();
    }
}