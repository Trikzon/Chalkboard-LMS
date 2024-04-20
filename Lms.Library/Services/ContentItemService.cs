using Lms.Library.Databases;
using Lms.Library.Models;

namespace Lms.Library.Services;

public class ContentItemService
{
    private static ContentItemService? _instance;
    public static ContentItemService Current => _instance ??= new ContentItemService();
    
    private readonly List<ContentItem> _contentItems = FakeDatabase.ContentItems;
    
    private ContentItemService() { }

    public void AddContentItem(ContentItem contentItem)
    {
        _contentItems.Add(contentItem);
    }

    public void RemoveContentItem(ContentItem contentItem)
    {
        _contentItems.Remove(contentItem);
    }

    public ContentItem? GetContentItem(Guid id)
    {
        return _contentItems.FirstOrDefault(contentItem => contentItem.Id == id);
    }
}