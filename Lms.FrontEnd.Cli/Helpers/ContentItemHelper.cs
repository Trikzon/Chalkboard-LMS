using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public class ContentItemHelper
{
    private readonly ContentItemService _contentItemService;
    private readonly ModuleService _moduleService;

    public ContentItemHelper(ContentItemService contentItemService, ModuleService moduleService)
    {
        _contentItemService = contentItemService;
        _moduleService = moduleService;
    }

    public void CreateContentItem(Module module)
    {
        var contentItem = new ContentItem();

        if (!UpdateName(contentItem))
            return;
        
        if (!UpdateDescription(contentItem))
            return;
        
        if (!UpdatePath(contentItem))
            return;

        contentItem.ModuleId = module.Id;
        module.Content.Add(contentItem.Id);
        _contentItemService.AddContentItem(contentItem);
        Console.WriteLine($"Successfully Created Content Item {contentItem}.");
    }

    public void ListContentItems(Module module)
    {
        Utils.DisplayList(module.Content.Select(id => _contentItemService.GetContentItem(id)).ToList());
    }

    public Guid? SelectContentItem(Module module)
    {
        var contentItems = module.Content.Select(id => _contentItemService.GetContentItem(id)).ToList();
        if (!Utils.TrySelectFromList("Content Item", contentItems, out var contentItem) || contentItem == null)
        {
            return null;
        }

        return contentItem.Id;
    }

    public bool UpdateName(ContentItem contentItem)
    {
        var name = Utils.ReadString("Name");
        if (name == "")
        {
            Console.WriteLine("Invalid Input.");
            return false;
        }

        contentItem.Name = name;
        return true;
    }

    public bool UpdateDescription(ContentItem contentItem)
    {
        contentItem.Description = Utils.ReadString("Description");
        return true;
    }

    public bool UpdatePath(ContentItem contentItem)
    {
        contentItem.Path = Utils.ReadString("Path");
        return true;
    }

    public void DeleteContentItem(CliProgram cli, ContentItem contentItem)
    {
        if (Utils.ConfirmDeletion("Content Item"))
        {
            _moduleService.GetModule(contentItem.ModuleId)?.Content.Remove(contentItem.Id);
            _contentItemService.RemoveContentItem(contentItem);
            Console.WriteLine("Successfully Deleted the Content Item.");
            cli.NavigateBack();
        }
    }
}