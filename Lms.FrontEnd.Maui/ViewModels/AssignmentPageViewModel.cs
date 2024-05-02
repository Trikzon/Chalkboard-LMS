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
    
    private string _submissionContent = "";
    
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
    
    public string SubmissionContent
    {
        get => _submissionContent;
        set
        {
            _submissionContent = value;
            OnPropertyChanged();
        }
    }
    
    public DateTime SubmissionDate { get; private set; }
    
    public float? Score { get; private set; }
    
    public bool IsSubmitted { get; private set; } = false;
        
    public bool IsGraded => Score is not null && IsSubmitted;
    
    public IEnumerable<SubmissionViewModel>? Submissions { get; private set; }
    
    public AssignmentPageViewModel(Guid contentItemId, Guid moduleId, Guid? studentId)
    {
        _contentItemId = contentItemId;
        _moduleId = moduleId;
        _studentId = studentId;
        
        Task.Run(async () =>
        {
            var assignment = await ContentItemService.Current.GetAssignmentAsync(_contentItemId);
            if (assignment is not null)
            {
                Name = assignment.Name;
                Content = assignment.Content ?? "";
                TotalAvailablePoints = assignment.TotalAvailablePoints;
                DueDate = assignment.DueDate;
            }
        });
    }

    public async Task UpdateAsync()
    {
        if (_studentId is not null)
        {
            var submission = await SubmissionService.Current.GetSubmissionAsync(_contentItemId, _studentId.Value);
            
            IsSubmitted = submission is not null;
            OnPropertyChanged(nameof(IsSubmitted));
            
            if (submission is not null)
            {
                SubmissionContent = submission.Content ?? "";
                OnPropertyChanged(nameof(SubmissionContent));
                SubmissionDate = submission.SubmissionDate;
                OnPropertyChanged(nameof(SubmissionDate));
                
                Score = submission.Points >= 0 ? submission.Points : null;
                OnPropertyChanged(nameof(Score));
                OnPropertyChanged(nameof(IsGraded));
            }
        }
        else
        {
            var submissions = await SubmissionService.Current.GetSubmissionsAsync(_contentItemId);
            if (submissions is not null)
            {
                Submissions = submissions.Select(s => new SubmissionViewModel(_contentItemId, s.StudentId));
                OnPropertyChanged(nameof(Submissions));
            }
        }
    }
    
    public async Task SaveChangesAsync()
    {
        await ModuleService.Current.UpdateAssignmentAsync(_moduleId, _contentItemId, Name, Content, TotalAvailablePoints, DueDate);
    }
    
    public async void SubmitAssignmentAsync()
    {
        await SubmissionService.Current.CreateSubmissionAsync(_contentItemId, _studentId!.Value, SubmissionContent, DateTime.Now, -1);
        IsSubmitted = true;
        OnPropertyChanged(nameof(IsSubmitted));
        SubmissionDate = DateTime.Now;
        OnPropertyChanged(nameof(SubmissionDate));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}