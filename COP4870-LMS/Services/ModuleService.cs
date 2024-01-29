using COP4870_LMS.Models;

namespace COP4870_LMS.Services;

public class ModuleService
{
    private readonly IList<Module> _modules;

    public ModuleService()
    {
        _modules = new List<Module>();
    }

    public ModuleService(IList<Module> modules)
    {
        _modules = modules;
    }

    public void AddModule(Module module)
    {
        _modules.Add(module);
    }

    public void RemoveModule(Module module)
    {
        _modules.Add(module);
    }

    public Module? GetModule(Guid id)
    {
        return _modules.FirstOrDefault(module => module.Id == id);
    }
}