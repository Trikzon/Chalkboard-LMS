using Lms.Library.Models;

namespace Lms.Library.Services;

public class ContentItemService
{
    private readonly IList<ContentItem> _contentItems;

    public ContentItemService()
    {
        _contentItems = new List<ContentItem>();
    }

    public ContentItemService(IList<ContentItem> contentItems)
    {
        _contentItems = contentItems;
    }

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