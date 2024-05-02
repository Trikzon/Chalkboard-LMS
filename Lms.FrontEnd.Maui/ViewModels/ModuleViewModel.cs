using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class ModuleViewModel : INotifyPropertyChanged
{
    public Guid ModuleId { get; }
    
    public string Name { get; set; } = "";
    
    public IEnumerable<ContentItemViewModel>? ContentItems { get; private set; }
    
    public ModuleViewModel(Guid moduleId)
    {
        ModuleId = moduleId;

        Task.Run(async () =>
        {
            var module = await ModuleService.Current.GetModuleAsync(ModuleId);
            if (module is not null)
            {
                Name = module.Name;
                OnPropertyChanged(nameof(Name));
            }
        });
        
        Task.Run(UpdateAsync);
    }

    private async Task UpdateAsync()
    {
        ContentItems = (await ModuleService.Current.GetContentItemsAsync(ModuleId) ?? [])
            .Select(c => new ContentItemViewModel(c.Id, ModuleId));
        OnPropertyChanged(nameof(ContentItems));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}