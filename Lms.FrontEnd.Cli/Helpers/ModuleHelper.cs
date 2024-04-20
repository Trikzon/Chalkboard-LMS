using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public static class ModuleHelper
{
    public static void CreateModule(Course course)
    {
        var module = new Module();
        
        if (!UpdateName(module))
            return;
        
        if (!UpdateDescription(module))
            return;

        module.CourseId = course.Id;
        course.Modules.Add(module.Id);
        ModuleService.Current.AddModule(module);
        Console.WriteLine($"Successfully Created Module {module}.");
    }

    public static void ListModules(Course course)
    {
        Utils.DisplayList(course.Modules.Select(id => ModuleService.Current.GetModule(id)).ToList());
    }

    public static Guid? SelectModule(Course course)
    {
        var modules = course.Modules.Select(id => ModuleService.Current.GetModule(id)).ToList();
        if (!Utils.TrySelectFromList("Module", modules, out var module) || module == null)
        {
            return null;
        }

        return module.Id;
    }
    
    public static bool UpdateName(Module module)
    {
        var name = Utils.ReadString("Name");
        if (name == "")
        {
            Console.WriteLine("Invalid Input.");
            return false;
        }

        module.Name = name;
        return true;
    }

    public static bool UpdateDescription(Module module)
    {
        module.Description = Utils.ReadString("Description");
        return true;
    }

    public static void DeleteModule(CliProgram cli, Module module)
    {
        if (!Utils.ConfirmDeletion("Module")) return;
        
        CourseService.Current.GetCourse(module.CourseId)?.Modules.Remove(module.Id);
        ModuleService.Current.RemoveModule(module);
        Console.WriteLine("Successfully Deleted the Module.");
        cli.NavigateBack();
    }
}