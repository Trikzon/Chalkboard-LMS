using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class AssignmentPage : ContentPage
{
    public ICommand OpenGradeAssignmentCommand { get; }
    
    public AssignmentPage(Guid contentItemId, Guid moduleId, Guid? studentId = null)
    {
        OpenGradeAssignmentCommand = new Command<SubmissionViewModel>(OpenGradeAssignment);
        
        InitializeComponent();
        BindingContext = new AssignmentPageViewModel(contentItemId, moduleId, studentId);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((AssignmentPageViewModel)BindingContext).UpdateAsync();
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await ((AssignmentPageViewModel)BindingContext).SaveChangesAsync();
    }

    private void SubmitAssignment_OnPressed(object? sender, EventArgs e)
    {
        ((AssignmentPageViewModel)BindingContext).SubmitAssignmentAsync();
    }
    
    private async void OpenGradeAssignment(SubmissionViewModel submission)
    {
        await Shell.Current.Navigation.PushAsync(new GradeAssignmentPage(submission));
    }
}