using Lms.Library.Databases;
using Lms.Library.Models;

namespace Lms.Library.Services;

public class ModuleService
{
    private static ModuleService? _instance;
    public static ModuleService Current => _instance ??= new ModuleService();

    private readonly List<Module> _modules = FakeDatabase.Modules;

    private ModuleService() { }

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