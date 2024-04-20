using Lms.Library.Models;

namespace Lms.Library.Databases;

public static class FakeDatabase
{
    public static List<Assignment> Assignments { get; } = [];
    public static List<ContentItem> ContentItems { get; } = [];
    public static List<Course> Courses { get; } = [];
    public static List<Module> Modules { get; } = [];
    public static List<Person> People { get; } = [];
}