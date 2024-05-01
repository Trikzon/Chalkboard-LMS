using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class EditStudentPageViewModel : INotifyPropertyChanged
{
    private readonly Guid _studentId;
    private string _name = "";
    private string _classification = "";
    
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    public string Classification
    {
        get => _classification;
        set
        {
            _classification = value;
            OnPropertyChanged();
        }
    }

    public EditStudentPageViewModel(Guid studentId)
    {
        _studentId = studentId;

        Task.Run(async () =>
        {
            var student = await StudentService.Current.GetStudentAsync(_studentId);
            if (student is not null)
            {
                Name = student.Name;
                OnPropertyChanged(nameof(Name));
                Classification = Enum.GetName(student.Classification) ?? "";
                OnPropertyChanged(nameof(Classification));
            }
        });
    }

    public async Task SaveChangesAsync()
    {
        await StudentService.Current.UpdateStudentAsync(_studentId, Name, Enum.Parse<Classification>(Classification));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}