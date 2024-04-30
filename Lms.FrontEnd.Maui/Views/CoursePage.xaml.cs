using Lms.FrontEnd.Maui.ViewModels;

namespace Lms.FrontEnd.Maui.Views;

public partial class CoursePage : ContentPage
{
    public CoursePage(Guid courseId)
    {
        InitializeComponent();
        BindingContext = new CoursePageViewModel(courseId);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((CoursePageViewModel)BindingContext).Update();
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        await ((CoursePageViewModel)BindingContext).Save();
    }
}