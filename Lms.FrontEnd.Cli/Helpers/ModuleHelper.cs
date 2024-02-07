using Lms.FrontEnd.Cli.CLI;
using Lms.Library.Models;
using Lms.Library.Services;

namespace Lms.FrontEnd.Cli.Helpers;

public class ModuleHelper
{
    private readonly ModuleService _moduleService;
    private readonly CourseService _courseService;

    public ModuleHelper(ModuleService moduleService, CourseService courseService)
    {
        _moduleService = moduleService;
        _courseService = courseService;
    }
    
    public void CreateModule(Course course)
    {
        var module = new Module();
        
        if (!UpdateName(module))
            return;
        
        if (!UpdateDescription(module))
            return;

        module.CourseId = course.Id;
        course.Modules.Add(module.Id);
        _moduleService.AddModule(module);
        Console.WriteLine($"Successfully Created Module {module}.");
    }

    public void ListModules(Course course)
    {
        Utils.DisplayList(course.Modules.Select(id => _moduleService.GetModule(id)).ToList());
    }

    public Guid? SelectModule(Course course)
    {
        var modules = course.Modules.Select(id => _moduleService.GetModule(id)).ToList();
        if (!Utils.TrySelectFromList("Module", modules, out var module) || module == null)
        {
            return null;
        }

        return module.Id;
    }
    
    public bool UpdateName(Module module)
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

    public bool UpdateDescription(Module module)
    {
        module.Description = Utils.ReadString("Description");
        return true;
    }

    public void DeleteModule(CliProgram cli, Module module)
    {
        if (Utils.ConfirmDeletion("Module"))
        {
            _courseService.GetCourse(module.CourseId)?.Modules.Remove(module.Id);
            _moduleService.RemoveModule(module);
            Console.WriteLine("Successfully Deleted the Module.");
            cli.NavigateBack();
        }
    }
}