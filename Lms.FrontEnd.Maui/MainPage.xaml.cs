using Lms.Library.Api.Services;
using Lms.Library.Contracts;

namespace Lms.FrontEnd.Maui;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await CourseService.Current.UpdateCourseAsync(
            new Guid("d6f30757-af98-4196-bcbd-a5596df65305"),
            new UpdateCourseRequest("New Course Name", "NEW", "New Course Description")
        );
        
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}