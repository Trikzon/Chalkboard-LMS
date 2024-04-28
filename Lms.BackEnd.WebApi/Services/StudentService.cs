using Lms.BackEnd.WebApi.Models;
using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class StudentService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;
    
    public void CreateStudent(Student student)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "INSERT INTO students (student_id, name, classification) VALUES (@id, @name, @classification)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", student.Id);
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@classification", Enum.GetName(student.Classification));
        
        command.ExecuteNonQuery();
    }
    
    public Student? GetStudent(Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT name, classification FROM students WHERE student_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", studentId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var name = reader.GetString(0);
        var classification = Enum.Parse<Classification>(reader.GetString(1));
        
        return new Student(studentId, name, classification);
    }
    
    public IEnumerable<Student> GetStudents()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT student_id, name, classification FROM students ORDER BY name";
        using var command = new MySqlCommand(query, connection);
        
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
    
    public bool UpdateStudent(Student student)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "UPDATE students SET name = @name, classification = @classification WHERE student_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", student.Id);
        command.Parameters.AddWithValue("@name", student.Name);
        command.Parameters.AddWithValue("@classification", Enum.GetName(student.Classification));
        
        return command.ExecuteNonQuery() > 0;
    }
    
    public void DeleteStudent(Guid studentId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string deleteEnrollmentsQuery = "DELETE FROM enrollments WHERE student_id = @id";
        using var deleteEnrollmentsCommand = new MySqlCommand(deleteEnrollmentsQuery, connection);
        deleteEnrollmentsCommand.Parameters.AddWithValue("@id", studentId);
        deleteEnrollmentsCommand.ExecuteNonQuery();
        
        const string deleteSubmissionsQuery = "DELETE FROM submissions WHERE student_id = @id";
        using var deleteSubmissionsCommand = new MySqlCommand(deleteSubmissionsQuery, connection);
        deleteSubmissionsCommand.Parameters.AddWithValue("@id", studentId);
        deleteSubmissionsCommand.ExecuteNonQuery();
        
        const string query = "DELETE FROM students WHERE student_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", studentId);
        command.ExecuteNonQuery();
    }
}