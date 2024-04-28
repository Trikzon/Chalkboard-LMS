using Lms.BackEnd.WebApi.Models;
using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class CourseService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;

    public void CreateCourse(Course course)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "INSERT INTO courses (course_id, name, code, description) VALUES (@id, @name, @code, @description)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", course.Id);
        command.Parameters.AddWithValue("@name", course.Name);
        command.Parameters.AddWithValue("@code", course.Code);
        command.Parameters.AddWithValue("@description", course.Description);
        
        command.ExecuteNonQuery();
    }
    
    public Course? GetCourse(Guid courseId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT name, code, description FROM courses WHERE course_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", courseId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var name = reader.GetString(0);
        var code = reader.GetString(1);
        var description = reader.IsDBNull(2) ? null : reader.GetString(2);
        
        return new Course(courseId, name, code, description);
    }
    
    public IEnumerable<Course> GetCourses()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT course_id, name, code, description FROM courses ORDER BY name";
        using var command = new MySqlCommand(query, connection);
        
        using var reader = command.ExecuteReader();
        var courses = new List<Course>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var code = reader.GetString(2);
            var description = reader.IsDBNull(3) ? null : reader.GetString(3);
            
            courses.Add(new Course(id, name, code, description));
        }
        
        return courses;
    }
    
    public bool UpdateCourse(Course course)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "UPDATE courses SET name = @name, code = @code, description = @description WHERE course_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", course.Id);
        command.Parameters.AddWithValue("@name", course.Name);
        command.Parameters.AddWithValue("@code", course.Code);
        command.Parameters.AddWithValue("@description", course.Description);
        
        return command.ExecuteNonQuery() > 0;
    }
    
    public void DeleteCourse(Guid courseId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string deleteEnrollmentsQuery = "DELETE FROM enrollments WHERE course_id = @id";
        using var deleteEnrollmentsCommand = new MySqlCommand(deleteEnrollmentsQuery, connection);
        deleteEnrollmentsCommand.Parameters.AddWithValue("@id", courseId);
        deleteEnrollmentsCommand.ExecuteNonQuery();
        
        const string deleteSubmissionsQuery = """
                                              DELETE FROM submissions WHERE content_item_id IN (
                                                SELECT content_item_id FROM assignments WHERE content_item_id IN (
                                                  SELECT content_item_id FROM content_items WHERE module_id IN (
                                                    SELECT module_id FROM modules WHERE course_id = @id
                                                  )
                                                )
                                              )
                                              """;
        using var deleteSubmissionsCommand = new MySqlCommand(deleteSubmissionsQuery, connection);
        deleteSubmissionsCommand.Parameters.AddWithValue("@id", courseId);
        deleteSubmissionsCommand.ExecuteNonQuery();
        
        const string deleteAssignmentsQuery = """
                                              DELETE FROM assignments WHERE content_item_id IN (
                                                SELECT content_item_id FROM content_items WHERE module_id IN (
                                                  SELECT module_id FROM modules WHERE course_id = @id
                                                )
                                              )
                                              """;
        using var deleteAssignmentsCommand = new MySqlCommand(deleteAssignmentsQuery, connection);
        deleteAssignmentsCommand.Parameters.AddWithValue("@id", courseId);
        deleteAssignmentsCommand.ExecuteNonQuery();
        
        const string deleteContentItemsQuery = """
                                               DELETE FROM content_items WHERE module_id IN (
                                                 SELECT module_id FROM modules WHERE course_id = @id
                                               )
                                               """;
        using var deleteContentItemsCommand = new MySqlCommand(deleteContentItemsQuery, connection);
        deleteContentItemsCommand.Parameters.AddWithValue("@id", courseId);
        deleteContentItemsCommand.ExecuteNonQuery();
        
        const string deleteModulesQuery = "DELETE FROM modules WHERE course_id = @id";
        using var deleteModulesCommand = new MySqlCommand(deleteModulesQuery, connection);
        deleteModulesCommand.Parameters.AddWithValue("@id", courseId);
        deleteModulesCommand.ExecuteNonQuery();
        
        const string query = "DELETE FROM courses WHERE course_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", courseId);
        command.ExecuteNonQuery();
    }
}