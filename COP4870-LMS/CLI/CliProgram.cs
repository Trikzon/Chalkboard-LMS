using System.Text;

namespace COP4870_LMS.CLI;

public class CliProgram
{
    public bool CanDisplay => _menus.Count > 0;
    
    private readonly Stack<Func<Menu>> _menus = new();

    public void NavigateTo(Func<Menu> menuBuilder)
    {
        _menus.Push(menuBuilder);
    }

    public void NavigateBack()
    {
        if (_menus.Count <= 0) return;
        
        _menus.Pop();
    }

    public void Display()
    {
        if (_menus.Count <= 0) return;
        
        _menus.Peek().Invoke().Display(this);
    }
    
    public void DisplayHeader(string option = "")
    {
        if (_menus.Count <= 0) return;
        
        Console.WriteLine();
        
        var header = option;
        foreach (var menu in _menus)
        {
            if (header != "")
                header = " > " + header;
            header = menu.Invoke().Name + header;
        }

        Console.WriteLine(header);
        for (var i = 0; i < header.Length; i++)
            Console.Write("-");
        Console.WriteLine();
    }
}