namespace Lms.FrontEnd.Cli.CLI;

public class Menu
{
    public string Name { get; }

    private readonly bool _goBack;
    private readonly List<string> _info = new();
    private readonly List<(string Name, Action<CliProgram> Action)> _options = new();

    public Menu(string name, bool goBack = true)
    {
        Name = name;
        _goBack = goBack;
    }

    public Menu AddInfo(string info)
    {
        _info.Add(info);
        return this;
    }

    public Menu AddOption(string name, Action<CliProgram> action)
    {
        _options.Add((name, action));
        return this;
    }
    
    public Menu AddOption(string name, Action action)
    {
        return AddOption(name, _ => action());
    }
    
    public Menu AddOption(string name, Func<CliProgram, Guid?> action, Func<Guid, Menu> menuBuilder)
    {
        return AddOption(name, cli =>
        {
            var selection = action(cli);
            if (selection.HasValue)
                cli.NavigateTo(() => menuBuilder(selection.Value));
        });
    }
    
    public void Display(CliProgram cli)
    {
        cli.DisplayHeader();

        if (_info.Count > 0)
        {
            foreach (var info in _info)
            {
                Console.WriteLine(info);
            }
            Console.WriteLine("---");
        }
        
        for (var i = 0; i < _options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_options[i].Name}");
        }

        if (_goBack)
        {
            Console.WriteLine($"{_options.Count + 1}. Go Back");
        }
        
        Console.Write("Enter an option: ");
        if (int.TryParse(Console.ReadLine()?.Trim(), out var input) && input > 0 && input <= _options.Count)
        {
            var option = _options[input - 1];
            cli.DisplayHeader(option.Name);
            option.Action.Invoke(cli);
        }
        else if (_goBack && input == _options.Count + 1)  // Go back option.
        {
            cli.NavigateBack();
        }
        else
        {
            Console.WriteLine($"Invalid input. Please enter an integer from 1 to {_options.Count + (_goBack ? 1 : 0)}.");
        }
    }
}