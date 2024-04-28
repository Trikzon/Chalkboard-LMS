using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class EnrollmentService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;
    
    public bool CreateEnrollment(Guid courseId, Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string courseQuery = "SELECT COUNT(*) FROM courses WHERE courses.course_id = @courseId";
        using var courseCommand = new MySqlCommand(courseQuery, connection);
        courseCommand.Parameters.AddWithValue("@courseId", courseId);
        
        if ((long)courseCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string studentQuery = "SELECT COUNT(*) FROM students WHERE students.student_id = @studentId";
        using var studentCommand = new MySqlCommand(studentQuery, connection);
        studentCommand.Parameters.AddWithValue("@studentId", studentId);
        
        if ((long)studentCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string query = "INSERT INTO enrollments (course_id, student_id) VALUES (@courseId, @studentId)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        command.Parameters.AddWithValue("@studentId", studentId);
        
        command.ExecuteNonQuery();
        
        return true;
    }
    
    public bool EnrollmentExists(Guid courseId, Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT COUNT(*) FROM enrollments WHERE course_id = @courseId AND student_id = @studentId";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        command.Parameters.AddWithValue("@studentId", studentId);
        
        return (long)command.ExecuteScalar() > 0;
    }
    
    public IEnumerable<Guid> GetEnrolledCourses(Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT course_id FROM enrollments WHERE student_id = @studentId";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@studentId", studentId);
        
        using var reader = command.ExecuteReader();
        var courseIds = new List<Guid>();
        while (reader.Read())
        {
            courseIds.Add(reader.GetGuid(0));
        }
        
        return courseIds;
    }
    
    public IEnumerable<Guid> GetEnrolledStudents(Guid courseId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT student_id FROM enrollments WHERE course_id = @courseId";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        
        using var reader = command.ExecuteReader();
        var studentIds = new List<Guid>();
        while (reader.Read())
        {
            studentIds.Add(reader.GetGuid(0));
        }
        
        return studentIds;
    }
    
    public void DeleteEnrollment(Guid courseId, Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "DELETE FROM enrollments WHERE course_id = @courseId AND student_id = @studentId";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        command.Parameters.AddWithValue("@studentId", studentId);
        
        command.ExecuteNonQuery();
    }
}