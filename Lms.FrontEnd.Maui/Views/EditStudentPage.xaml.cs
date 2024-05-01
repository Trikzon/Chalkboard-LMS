using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class EditStudentPage : ContentPage
{
    public EditStudentPage(Guid studentId)
    {
        InitializeComponent();
        BindingContext = new EditStudentPageViewModel(studentId);
        
        foreach (var classification in Enum.GetValues<Classification>())
        {
            ClassificationPicker.Items.Add(classification.ToString());
        }
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        await ((EditStudentPageViewModel)BindingContext).SaveChangesAsync();
    }
}