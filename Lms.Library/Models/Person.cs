namespace Lms.Library.Models;

public class Person
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public Classification Classification { get; set; } = Classification.Freshman;
    public double Grades { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Classification})";
    }
    
    private static readonly Dictionary<string, Classification> StringToClassification = new()
    {
        { "freshman", Classification.Freshman },
        { "sophomore", Classification.Sophomore },
        { "junior", Classification.Junior },
        { "senior", Classification.Senior },
        { "graduate", Classification.Graduate }
    };

    public bool TryParseClassification(string value)
    {
        if (StringToClassification.TryGetValue(value.ToLower(), out var classification))
        {
            Classification = classification;
            return true;
        }

        return false;
    }
}