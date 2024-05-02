using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lms.Library.Api.Services;

namespace Lms.FrontEnd.Maui.ViewModels;

public sealed class SubmissionViewModel : INotifyPropertyChanged
{
    private readonly Guid _contentItemId;
    private readonly Guid _studentId;

    public string StudentName { get; set; } = "";
    
    public string SubmissionContent { get; set; } = "";
    
    public DateTime SubmissionDate { get; set; }
    
    public float? Score { get; set; }
    
    public int TotalAvailablePoints { get; set; }

    public bool IsGraded => Score is not null;
    
    public SubmissionViewModel(Guid contentItemId, Guid studentId)
    {
        _contentItemId = contentItemId;
        _studentId = studentId;

        Task.Run(async () =>
        {
            var submission = await SubmissionService.Current.GetSubmissionAsync(_contentItemId, _studentId);
            if (submission is not null)
            {
                SubmissionContent = submission.Content ?? "";
                SubmissionDate = submission.SubmissionDate;
                Score = submission.Points >= 0 ? submission.Points : null;
                
                var student = await StudentService.Current.GetStudentAsync(_studentId);
                if (student is not null)
                {
                    StudentName = student.Name;
                    OnPropertyChanged(nameof(StudentName));
                }
                
                var assignment = await ContentItemService.Current.GetAssignmentAsync(_contentItemId);
                if (assignment is not null)
                {
                    TotalAvailablePoints = assignment.TotalAvailablePoints;
                    OnPropertyChanged(nameof(TotalAvailablePoints));
                }
                
                OnPropertyChanged(nameof(SubmissionContent));
                OnPropertyChanged(nameof(SubmissionDate));
                OnPropertyChanged(nameof(Score));
                OnPropertyChanged(nameof(IsGraded));
            }
        });
    }
    
    public async Task SaveChangesAsync()
    {
        await SubmissionService.Current.UpdateSubmissionAsync(_contentItemId, _studentId, SubmissionContent, SubmissionDate, Score ?? -1);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}