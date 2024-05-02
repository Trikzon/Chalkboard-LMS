using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class AssignmentPageViewModel : INotifyPropertyChanged
{
    private readonly Guid _contentItemId;
    private readonly Guid _moduleId;
    private readonly Guid? _studentId;
    private string _name = "";
    private string _content = "";
    private int _totalAvailablePoints;
    private DateTime _dueDate;
    
    public bool IsInstructor => _studentId is null;
    
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged();
        }
    }
    
    public int TotalAvailablePoints
    {
        get => _totalAvailablePoints;
        set
        {
            _totalAvailablePoints = value;
            OnPropertyChanged();
        }
    }
    
    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            _dueDate = value;
            OnPropertyChanged();
        }
    }
    
    public AssignmentPageViewModel(Guid contentItemId, Guid moduleId, Guid? studentId)
    {
        _contentItemId = contentItemId;
        _moduleId = moduleId;
        _studentId = studentId;
        
        Task.Run(async () =>
        {
            var assignment = await ContentItemService.Current.GetAssignmentAsync(_contentItemId);;;;
            if (assignment is not null)
            {
                Name = assignment.Name;
                Content = assignment.Content ?? "";
                TotalAvailablePoints = assignment.TotalAvailablePoints;
                DueDate = assignment.DueDate;
            }
        });
    }
    
    public async Task SaveChangesAsync()
    {
        await ModuleService.Current.UpdateAssignmentAsync(_moduleId, _contentItemId, Name, Content, TotalAvailablePoints, DueDate);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}