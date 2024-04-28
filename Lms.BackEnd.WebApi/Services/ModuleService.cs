using Lms.BackEnd.WebApi.Models;
using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class ModuleService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;
    
    public bool CreateModule(Module module)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string courseQuery = "SELECT COUNT(*) FROM courses WHERE courses.course_id = @courseId";
        using var courseCommand = new MySqlCommand(courseQuery, connection);
        courseCommand.Parameters.AddWithValue("@courseId", module.CourseId);
        
        if ((long)courseCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string query = "INSERT INTO modules (module_id, course_id, name) VALUES (@id, @course_id, @name)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", module.Id);
        command.Parameters.AddWithValue("@course_id", module.CourseId);
        command.Parameters.AddWithValue("@name", module.Name);
        
        command.ExecuteNonQuery();

        return true;
    }
    
    public Module? GetModule(Guid moduleId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT course_id, name FROM modules WHERE module_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", moduleId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var courseId = reader.GetGuid(0);
        var name = reader.GetString(1);
        
        return new Module(moduleId, courseId, name);
    }
    
    public IEnumerable<Module> GetModules(Guid courseId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT module_id, name FROM modules WHERE course_id = @courseId ORDER BY name";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@courseId", courseId);
        
        using var reader = command.ExecuteReader();
        var modules = new List<Module>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            
            modules.Add(new Module(id, courseId, name));
        }
        
        return modules;
    }
    
    public bool UpdateModule(Module module)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "UPDATE modules SET name = @name WHERE module_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", module.Id);
        command.Parameters.AddWithValue("@name", module.Name);
        
        return command.ExecuteNonQuery() > 0;
    }
    
    public void DeleteModule(Guid moduleId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string deleteSubmissionsQuery = """
                                              DELETE FROM submissions WHERE content_item_id IN (
                                                SELECT content_item_id FROM assignments WHERE content_item_id IN (
                                                  SELECT content_item_id FROM content_items WHERE module_id = @id
                                                )
                                              )
                                              """;
        using var deleteSubmissionsCommand = new MySqlCommand(deleteSubmissionsQuery, connection);
        deleteSubmissionsCommand.Parameters.AddWithValue("@id", moduleId);
        deleteSubmissionsCommand.ExecuteNonQuery();
        
        const string deleteAssignmentsQuery = """
                                              DELETE FROM assignments WHERE content_item_id IN (
                                                SELECT content_item_id FROM content_items WHERE module_id = @id
                                              )
                                              """;
        using var deleteAssignmentsCommand = new MySqlCommand(deleteAssignmentsQuery, connection);
        deleteAssignmentsCommand.Parameters.AddWithValue("@id", moduleId);
        deleteAssignmentsCommand.ExecuteNonQuery();
        
        const string deleteContentItemsQuery = "DELETE FROM content_items WHERE module_id = @id";
        using var deleteContentItemsCommand = new MySqlCommand(deleteContentItemsQuery, connection);
        deleteContentItemsCommand.Parameters.AddWithValue("@id", moduleId);
        deleteContentItemsCommand.ExecuteNonQuery();
        
        const string query = "DELETE FROM modules WHERE module_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", moduleId);
        
        command.ExecuteNonQuery();
    }
}