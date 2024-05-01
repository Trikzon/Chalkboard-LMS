using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lms.Library.Api.Services;
using Lms.Library.Models;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class StudentLoginPageViewModel : INotifyPropertyChanged
{
    private IEnumerable<Student>? _students;
    private string _searchQuery = "";

    public IEnumerable<Student>? Students
    {
        get => _students?.Where(c => c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        private set => _students = value;
    }

    public Student? SelectedStudent { get; set; }
    
    public bool IsStudentSelected => SelectedStudent != null;
    
    public ICommand SelectionChangedCommand => new Command(() => OnPropertyChanged(nameof(IsStudentSelected)));

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            SelectedStudent = null;
            OnPropertyChanged(nameof(Students));
            OnPropertyChanged(nameof(IsStudentSelected));
        }
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