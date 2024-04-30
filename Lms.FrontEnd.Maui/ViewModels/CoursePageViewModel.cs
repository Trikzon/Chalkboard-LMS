using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Contracts;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class CoursePageViewModel : INotifyPropertyChanged
{
    private readonly Guid _courseId;
    private string _name = "";
    private string _code = "";
    private string _description = "";

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

    public CoursePageViewModel(Guid courseId)
    {
        _courseId = courseId;
        Task.Run(Update);
    }
    
    public async Task Update()
    {
        var course = await CourseService.Current.GetCourseAsync(_courseId);
        if (course is not null)
        {
            Name = course.Name;
            Code = course.Code;
            Description = course.Description ?? "";
        }
    }
    
    public async Task Save()
    {
        await CourseService.Current.UpdateCourseAsync(_courseId, new UpdateCourseRequest(Name, Code, Description));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}