using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class StudentLoginPageViewModel : INotifyPropertyChanged
{
    public IEnumerable<Student>? Students { get; private set; }
    
    public Student? SelectedStudent { get; set; }
    
    public bool IsStudentSelected => SelectedStudent != null;
    
    public ICommand SelectionChangedCommand => new Command(() => OnPropertyChanged(nameof(IsStudentSelected)));

    public StudentLoginPageViewModel()
    {
        Task.Run(UpdateAsync);
    }

    public async Task UpdateAsync()
    {
        Students = await StudentService.Current.GetStudentsAsync();
        OnPropertyChanged(nameof(Students));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}