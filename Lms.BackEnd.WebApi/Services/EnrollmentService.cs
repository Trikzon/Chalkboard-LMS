using Lms.Library.Models;
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
    
    public IEnumerable<Course> GetEnrolledCourses(Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = """
                             SELECT courses.course_id, courses.name, courses.code, courses.description
                             FROM courses
                             JOIN enrollments
                                ON courses.course_id = enrollments.course_id
                                WHERE enrollments.student_id = @studentId
                             """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@studentId", studentId);
        
        using var reader = command.ExecuteReader();
        var courses = new List<Course>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var code = reader.GetString(2);
            var description = reader.GetString(3);
            
            courses.Add(new Course(id, name, code, description));
        }
        
        return courses;
    }
    
    public IEnumerable<Student> GetEnrolledStudents(Guid courseId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = """
                             SELECT students.student_id, students.name, students.classification
                             FROM students
                             JOIN enrollments
                                ON students.student_id = enrollments.student_id
                                WHERE enrollments.course_id = @courseId
                             """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        
        using var reader = command.ExecuteReader();
        var students = new List<Student>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var classification = Enum.Parse<Classification>(reader.GetString(2));
            
            students.Add(new Student(id, name, classification));
        }
        
        return students;
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