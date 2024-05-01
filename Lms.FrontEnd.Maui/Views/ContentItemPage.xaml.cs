using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class ContentItemPage : ContentPage
{
    public ContentItemPage(Guid contentItemId, Guid moduleId, Guid? studentId = null)
    {
        InitializeComponent();
        BindingContext = new ContentItemPageViewModel(contentItemId, moduleId, studentId);
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await ((ContentItemPageViewModel)BindingContext).SaveChangesAsync();
    }
}