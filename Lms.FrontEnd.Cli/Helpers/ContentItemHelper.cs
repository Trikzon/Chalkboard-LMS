using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public static class ContentItemHelper
{
    public static void CreateContentItem(Module module)
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
        ContentItemService.Current.AddContentItem(contentItem);
        Console.WriteLine($"Successfully Created Content Item {contentItem}.");
    }

    public static void ListContentItems(Module module)
    {
        Utils.DisplayList(module.Content.Select(id => ContentItemService.Current.GetContentItem(id)).ToList());
    }

    public static Guid? SelectContentItem(Module module)
    {
        var contentItems = module.Content.Select(id => ContentItemService.Current.GetContentItem(id)).ToList();
        if (!Utils.TrySelectFromList("Content Item", contentItems, out var contentItem) || contentItem == null)
        {
            return null;
        }

        return contentItem.Id;
    }

    public static bool UpdateName(ContentItem contentItem)
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

    public static bool UpdateDescription(ContentItem contentItem)
    {
        contentItem.Description = Utils.ReadString("Description");
        return true;
    }

    public static bool UpdatePath(ContentItem contentItem)
    {
        contentItem.Path = Utils.ReadString("Path");
        return true;
    }

    public static void DeleteContentItem(CliProgram cli, ContentItem contentItem)
    {
        if (!Utils.ConfirmDeletion("Content Item")) return;
        
        ModuleService.Current.GetModule(contentItem.ModuleId)?.Content.Remove(contentItem.Id);
        ContentItemService.Current.RemoveContentItem(contentItem);
        Console.WriteLine("Successfully Deleted the Content Item.");
        cli.NavigateBack();
    }
}