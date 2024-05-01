using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class ContentItemPageViewModel : INotifyPropertyChanged
{
    private readonly Guid _contentItemId;
    private readonly Guid _moduleId;
    private readonly Guid? _studentId;
    private string _name = "";
    private string _content = "";
    
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
    
    public ContentItemPageViewModel(Guid contentItemId, Guid moduleId, Guid? studentId)
    {
        _contentItemId = contentItemId;
        _moduleId = moduleId;
        _studentId = studentId;
        
        Task.Run(async () =>
        {
            var contentItem = await ContentItemService.Current.GetContentItemAsync(contentItemId);
            if (contentItem is not null)
            {
                Name = contentItem.Name;
                Content = contentItem.Content ?? "";
            }
        });
    }

    public async Task SaveChangesAsync()
    {
        await ModuleService.Current.UpdateContentItemAsync(_moduleId, _contentItemId, Name, Content);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}