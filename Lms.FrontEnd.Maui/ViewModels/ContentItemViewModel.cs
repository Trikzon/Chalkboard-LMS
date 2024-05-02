using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class ContentItemViewModel : INotifyPropertyChanged
{
    public Guid ContentItemId { get; }
    public Guid ModuleId { get; }
    
    public string Name { get; set; } = "";

    public bool IsAssignment { get; private set; } = false;
    
    public ContentItemViewModel(Guid contentItemId, Guid moduleId)
    {
        ContentItemId = contentItemId;
        ModuleId = moduleId;

        Task.Run(async () =>
        {
            var contentItem = await ContentItemService.Current.GetContentItemAsync(ContentItemId);
            if (contentItem is not null)
            {
                Name = contentItem.Name;
                OnPropertyChanged(nameof(Name));
            }
            
            var assignment = await ContentItemService.Current.GetAssignmentAsync(ContentItemId);
            IsAssignment = assignment is not null;
        });
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}