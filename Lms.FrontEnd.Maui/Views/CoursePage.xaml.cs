using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class CoursePage : ContentPage
{
    public ICommand DeleteStudentEnrollmentCommand { get; }
    
    public CoursePage(Guid courseId, Guid? studentId = null)
    {
        DeleteStudentEnrollmentCommand = new Command<Student>(DeleteStudentEnrollment);
        
        InitializeComponent();
        BindingContext = new CoursePageViewModel(courseId, studentId);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await ((CoursePageViewModel)BindingContext).UpdateAsync();
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        await ((CoursePageViewModel)BindingContext).SaveChangesAsync();
    }
    
    private async void DeleteStudentEnrollment(Student student)
    {
        var result = await DisplayAlert(
            "Remove Student Enrollment",
            $"Are you sure you want to remove\n{student.Name}?",
            "Yes", "No"
        );

        if (result)
        {
            await ((CoursePageViewModel)BindingContext).DeleteStudentEnrollmentAsync(student);
        }
    }

    private async void EnrollStudents_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.Navigation.PushAsync(new EnrollStudentsPage(((CoursePageViewModel)BindingContext).CourseId));
    }
}