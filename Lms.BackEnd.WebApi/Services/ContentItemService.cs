using Lms.Library.Models;
using MySql.Data.MySqlClient;

namespace Lms.BackEnd.WebApi.Services;

public class ContentItemService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("MySQLConnection")!;
    
    public bool CreateContentItem(ContentItem contentItem)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string moduleQuery = "SELECT COUNT(*) FROM modules WHERE modules.module_id = @moduleId";
        using var moduleCommand = new MySqlCommand(moduleQuery, connection);
        moduleCommand.Parameters.AddWithValue("@moduleId", contentItem.ModuleId);
        
        if ((long)moduleCommand.ExecuteScalar() == 0)
        {
            return false;
        }
        
        const string query = "INSERT INTO content_items (content_item_id, module_id, name, content) VALUES (@id, @module_id, @name, @content)";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", contentItem.Id);
        command.Parameters.AddWithValue("@module_id", contentItem.ModuleId);
        command.Parameters.AddWithValue("@name", contentItem.Name);
        command.Parameters.AddWithValue("@content", contentItem.Content);
        
        command.ExecuteNonQuery();

        if (contentItem is Assignment assignment)
        {
            const string assignmentQuery = "INSERT INTO assignments (content_item_id, total_available_points, due_date) VALUES (@id, @total_available_points, @due_date)";
            using var assignmentCommand = new MySqlCommand(assignmentQuery, connection);
            assignmentCommand.Parameters.AddWithValue("@id", assignment.Id);
            assignmentCommand.Parameters.AddWithValue("@total_available_points", assignment.TotalAvailablePoints);
            assignmentCommand.Parameters.AddWithValue("@due_date", assignment.DueDate);
            
            assignmentCommand.ExecuteNonQuery();
        }

        return true;
    }
    
    public ContentItem? GetContentItem(Guid contentItemId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT module_id, name, content FROM content_items WHERE content_item_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", contentItemId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var moduleId = reader.GetGuid(0);
        var name = reader.GetString(1);
        var content = reader.IsDBNull(2) ? null : reader.GetString(2);
        
        return new ContentItem(contentItemId, moduleId, name, content);
    }
    
    public Assignment? GetAssignment(Guid contentItemId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT module_id, name, content, total_available_points, due_date FROM content_items JOIN assignments ON content_items.content_item_id = assignments.content_item_id WHERE content_items.content_item_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", contentItemId);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var moduleId = reader.GetGuid(0);
        var name = reader.GetString(1);
        var content = reader.IsDBNull(2) ? null : reader.GetString(2);
        var totalAvailablePoints = reader.GetInt32(3);
        var dueDate = reader.GetDateTime(4);
        
        return new Assignment(contentItemId, moduleId, name, content, totalAvailablePoints, dueDate);
    }
    
    public IEnumerable<ContentItem> GetContentItems(Guid moduleId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "SELECT content_item_id, name, content FROM content_items WHERE module_id = @moduleId ORDER BY name";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@moduleId", moduleId);
        
        using var reader = command.ExecuteReader();
        var contentItems = new List<ContentItem>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var content = reader.IsDBNull(2) ? null : reader.GetString(2);
            
            contentItems.Add(new ContentItem(id, moduleId, name, content));
        }
        
        return contentItems;
    }

    public IEnumerable<Assignment> GetAssignments(Guid moduleId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = """
                             SELECT content_items.content_item_id, name, content, total_available_points, due_date
                             FROM content_items
                             JOIN assignments
                                ON content_items.content_item_id = assignments.content_item_id
                                WHERE module_id = @moduleId ORDER BY name
                             """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@moduleId", moduleId);
        
        using var reader = command.ExecuteReader();
        var assignments = new List<Assignment>();
        while (reader.Read())
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var content = reader.IsDBNull(2) ? null : reader.GetString(2);
            var totalAvailablePoints = reader.GetInt32(3);
            var dueDate = reader.GetDateTime(4);
            
            assignments.Add(new Assignment(id, moduleId, name, content, totalAvailablePoints, dueDate));
        }
        
        return assignments;
    }
    
    public bool UpdateContentItem(ContentItem contentItem)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string query = "UPDATE content_items SET name = @name, content = @content WHERE content_item_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", contentItem.Id);
        command.Parameters.AddWithValue("@name", contentItem.Name);
        command.Parameters.AddWithValue("@content", contentItem.Content);
        
        command.ExecuteNonQuery();

        if (contentItem is Assignment assignment)
        {
            const string assignmentQuery = "UPDATE assignments SET total_available_points = @total_available_points, due_date = @due_date WHERE content_item_id = @id";
            using var assignmentCommand = new MySqlCommand(assignmentQuery, connection);
            assignmentCommand.Parameters.AddWithValue("@id", assignment.Id);
            assignmentCommand.Parameters.AddWithValue("@total_available_points", assignment.TotalAvailablePoints);
            assignmentCommand.Parameters.AddWithValue("@due_date", assignment.DueDate);
            
            assignmentCommand.ExecuteNonQuery();
        }

        return true;
    }
    
    public void DeleteContentItem(Guid contentItemId)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string submissionQuery = "DELETE FROM submissions WHERE content_item_id = @id";
        using var submissionCommand = new MySqlCommand(submissionQuery, connection);
        submissionCommand.Parameters.AddWithValue("@id", contentItemId);
        submissionCommand.ExecuteNonQuery();
        
        const string assignmentQuery = "DELETE FROM assignments WHERE content_item_id = @id";
        using var assignmentCommand = new MySqlCommand(assignmentQuery, connection);
        assignmentCommand.Parameters.AddWithValue("@id", contentItemId);
        assignmentCommand.ExecuteNonQuery();
        
        const string query = "DELETE FROM content_items WHERE content_item_id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", contentItemId);
        command.ExecuteNonQuery();
        
        assignmentCommand.ExecuteNonQuery();
    }
}