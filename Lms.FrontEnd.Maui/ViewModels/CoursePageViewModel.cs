using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CoursePageViewModel : INotifyPropertyChanged
{
    private string _name = "";
    private string _code = "";
    private string _description = "";
    
    public Guid CourseId { get; }
    public Guid? StudentId { get; }

    public bool IsInstructor => StudentId is null;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }
    
    public IEnumerable<Student>? Roster { get; private set; }
    
    public IEnumerable<ModuleViewModel>? Modules { get; private set; }

    public CoursePageViewModel(Guid courseId, Guid? studentId)
    {
        CourseId = courseId;
        StudentId = studentId;
        
        Task.Run(async () =>
        {
            var course = await CourseService.Current.GetCourseAsync(CourseId);
            if (course is not null)
            {
                Name = course.Name;
                Code = course.Code;
                Description = course.Description ?? "";
            }
        });
    }
    
    public async Task UpdateAsync()
    {
        Roster = await CourseService.Current.GetEnrolledStudentsAsync(CourseId);
        OnPropertyChanged(nameof(Roster));
        Modules = (await CourseService.Current.GetModulesAsync(CourseId) ?? []).Select(m => new ModuleViewModel(m.Id));
        OnPropertyChanged(nameof(Modules));
    }
    
    public async Task SaveChangesAsync()
    {
        await CourseService.Current.UpdateCourseAsync(CourseId, Name, Code, Description);
    }
    
    public async Task DeleteStudentEnrollmentAsync(Student student)
    {
        await CourseService.Current.DeleteEnrollmentAsync(CourseId, student.Id);
        await UpdateAsync();
    }
    
    public async Task CreateModuleAsync(string name)
    {
        await CourseService.Current.CreateModuleAsync(CourseId, name);
        await UpdateAsync();
    }

    public async Task UpdateModuleNameAsync(ModuleViewModel module, string name)
    {
        await CourseService.Current.UpdateModuleAsync(CourseId, module.ModuleId, name);
        await UpdateAsync();
    }

    public async Task DeleteModuleAsync(ModuleViewModel module)
    {
        await ModuleService.Current.DeleteModuleAsync(module.ModuleId);
        await UpdateAsync();
    }
    
    public async Task<ContentItem?> CreateContentItemAsync(ModuleViewModel module)
    {
        var contentItem = await ModuleService.Current.CreateContentItemAsync(module.ModuleId, "New Item");
        await UpdateAsync();
        return contentItem;
    }
    
    public async Task<Assignment?> CreateAssignmentAsync(ModuleViewModel module)
    {
        var assignment = await ModuleService.Current.CreateAssignmentAsync(module.ModuleId, "New Assignment", "", 0, DateTime.Now);
        await UpdateAsync();
        return assignment;
    }
    
    public async Task DeleteContentItemAsync(ContentItemViewModel contentItem)
    {
        await ContentItemService.Current.DeleteContentItemAsync(contentItem.ContentItemId);
        await UpdateAsync();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}