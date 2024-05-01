using System.Windows.Input;
using Lms.FrontEnd.Maui.ViewModels;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.Views;

public partial class CoursePage : ContentPage
{
    public ICommand DeleteStudentEnrollmentCommand { get; }
    
    public ICommand EditModuleNameCommand { get; }
    public ICommand DeleteModuleCommand { get; }
    
    public ICommand CreateContentItemCommand { get; }
    public ICommand EditContentItemCommand { get; }
    public ICommand DeleteContentItemCommand { get; }
    
    public CoursePage(Guid courseId, Guid? studentId = null)
    {
        DeleteStudentEnrollmentCommand = new Command<Student>(DeleteStudentEnrollment);
        
        EditModuleNameCommand = new Command<ModuleViewModel>(EditModuleName);
        DeleteModuleCommand = new Command<ModuleViewModel>(DeleteModule);
        
        CreateContentItemCommand = new Command<ModuleViewModel>(CreateContentItem);
        EditContentItemCommand = new Command<ContentItem>(EditContentItem);
        DeleteContentItemCommand = new Command<ContentItem>(DeleteContentItem);
        
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

    private async void CreateModule_OnClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Create Module", "Enter the name of the module.", "Save");
        
        if (!string.IsNullOrWhiteSpace(name))
        {
            await ((CoursePageViewModel)BindingContext).CreateModuleAsync(name);
        }
    }

    private async void EditModuleName(ModuleViewModel module)
    {
        var name = await DisplayPromptAsync("Edit Module", "Enter the new name of the module.", "Save", initialValue: module.Name);
        
        if (!string.IsNullOrWhiteSpace(name))
        {
            await ((CoursePageViewModel)BindingContext).UpdateModuleNameAsync(module, name);
        }
    }

    private async void DeleteModule(ModuleViewModel module)
    {
        var result = await DisplayAlert(
            "Delete Module",
            $"Are you sure you want to delete\n{module.Name}?\nThis action cannot be undone.",
            "Yes", "No"
        );

        if (result)
        {
            await ((CoursePageViewModel)BindingContext).DeleteModuleAsync(module);
        }
    }
    
    private async void CreateContentItem(ModuleViewModel module)
    {
        var contentItem = await ((CoursePageViewModel)BindingContext).CreateContentItemAsync(module);
        
        if (contentItem is not null)
        {
            await Shell.Current.Navigation.PushAsync(new ContentItemPage(contentItem.Id, module.ModuleId));
        }
    }
    
    private async void EditContentItem(ContentItem contentItem)
    {
        await Shell.Current.Navigation.PushAsync(new ContentItemPage(contentItem.Id, contentItem.ModuleId, ((CoursePageViewModel)BindingContext).StudentId));
    }

    private async void DeleteContentItem(ContentItem contentItem)
    {
        var result = await DisplayAlert(
            "Delete Content Item",
            $"Are you sure you want to delete\n{contentItem.Name}?\nThis action cannot be undone.",
            "Yes", "No"
        );

        if (result)
        {
            await ((CoursePageViewModel)BindingContext).DeleteContentItemAsync(contentItem);
        }
    }
}