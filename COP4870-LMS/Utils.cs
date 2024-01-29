namespace COP4870_LMS;

public static class Utils
{
    public static string ReadString(string name)
    {
        Console.Write($"Enter the {name}: ");
        return Console.ReadLine()?.Trim() ?? "";
    }
    
    public static void DisplayList<T>(IReadOnlyList<T> list, bool wait = true)
    {
        for (var i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {list[i]}");
        }

        if (list.Count == 0)
        {
            Console.WriteLine("No Results.");
        }

        if (wait)
        {
            Console.Write("Press [Enter] to Continue: ");
            Console.ReadLine();
        }
    }

    public static bool ConfirmDeletion(string name)
    {
        Console.Write($"Are You Sure You Want to DELETE This {name}? y/N: ");
        return (Console.ReadLine()?.Trim().ToLower() ?? "") == "y";
    }
    
    public static bool TrySelectFromList<T>(string name, IReadOnlyList<T> list, out T? selection)
    {
        selection = default;
        
        DisplayList(list, wait: list.Count == 0);

        if (list.Count == 0)
        {
            return false;
        }
        
        if (!int.TryParse(ReadString($"{name}'s Index"), out var input) || input < 0 || input > list.Count)
        {
            Console.WriteLine($"Invalid input. Please enter an integer from 1 to {list.Count}.");
            return false;
        }

        selection = list[input - 1];
        return true;
    }
}