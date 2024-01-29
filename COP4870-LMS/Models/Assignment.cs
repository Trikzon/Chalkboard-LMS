namespace COP4870_LMS.Models;

public class Assignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid CourseId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public double TotalAvailablePoints { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UnixEpoch;

    public override string ToString()
    {
        return $"{Name} (Due {DueDate.ToShortDateString()})";
    }
}