using Lms.Library.Models;
using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class SubmissionService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;
    
    public bool CreateSubmission(Submission submission)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string assignmentQuery = "SELECT COUNT(*) FROM assignments WHERE assignments.content_item_id = @contentItemId";
        using var assignmentCommand = new MySqlCommand(assignmentQuery, connection);
        assignmentCommand.Parameters.AddWithValue("@contentItemId", submission.ContentItemId);
        
        if ((long)assignmentCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string studentQuery = "SELECT COUNT(*) FROM students WHERE students.student_id = @studentId";
        using var studentCommand = new MySqlCommand(studentQuery, connection);
        studentCommand.Parameters.AddWithValue("@studentId", submission.StudentId);
        
        if ((long)studentCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string query = "INSERT INTO submissions (content_item_id, student_id, content, submission_date, points) VALUES (@content_item_id, @student_id, @content, @submission_date, @points)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@content_item_id", submission.ContentItemId);
        command.Parameters.AddWithValue("@student_id", submission.StudentId);
        command.Parameters.AddWithValue("@content", submission.Content);
        command.Parameters.AddWithValue("@submission_date", submission.SubmissionDate);
        command.Parameters.AddWithValue("@points", submission.Points);
        
        command.ExecuteNonQuery();

        return true;
    }
    
    public Submission? GetSubmission(Guid contentItemId, Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT content, submission_date, points FROM submissions WHERE content_item_id = @content_item_id AND student_id = @student_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@content_item_id", contentItemId);
        command.Parameters.AddWithValue("@student_id", studentId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Submission(
            contentItemId,
            studentId,
            reader.GetString("content"),
            reader.GetDateTime("submission_date"),
            reader.GetFloat("points")
        );
    }
    
    public IEnumerable<Submission> GetSubmissions(Guid contentItemId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT student_id, content, submission_date, points FROM submissions WHERE content_item_id = @content_item_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@content_item_id", contentItemId);
        
        using var reader = command.ExecuteReader();
        var submissions = new List<Submission>();
        while (reader.Read())
        {
            submissions.Add(new Submission(
                contentItemId,
                reader.GetGuid("student_id"),
                reader.GetString("content"),
                reader.GetDateTime("submission_date"),
                reader.GetFloat("points")
            ));
        }

        return submissions;
    }
    
    public bool UpdateSubmission(Submission submission)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "UPDATE submissions SET content = @content, submission_date = @submission_date, points = @points WHERE content_item_id = @content_item_id AND student_id = @student_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@content_item_id", submission.ContentItemId);
        command.Parameters.AddWithValue("@student_id", submission.StudentId);
        command.Parameters.AddWithValue("@content", submission.Content);
        command.Parameters.AddWithValue("@submission_date", submission.SubmissionDate);
        command.Parameters.AddWithValue("@points", submission.Points);
        
        return command.ExecuteNonQuery() > 0;
    }
    
    public bool DeleteSubmission(Guid contentItemId, Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "DELETE FROM submissions WHERE content_item_id = @content_item_id AND student_id = @student_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@content_item_id", contentItemId);
        command.Parameters.AddWithValue("@student_id", studentId);
        
        return command.ExecuteNonQuery() > 0;
    }
}