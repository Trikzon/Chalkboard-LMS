namespace Lms.BackEnd.WebApi.Models;

public class Submission(Guid contentItemId, Guid studentId, string? content, DateTime submissionDate, float points)
{
    public Guid ContentItemId { get; } = contentItemId;
    public Guid StudentId { get; } = studentId;
    public string? Content { get; } = content;
    public DateTime SubmissionDate { get; } = submissionDate;
    public float Points { get; } = points;
}