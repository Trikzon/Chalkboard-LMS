using Lms.Library.Databases;
using Lms.Library.Models;

namespace Lms.Library.Services;

public class PersonService
{
    private static PersonService? _instance;
    public static PersonService Current => _instance ??= new PersonService();

    private readonly List<Person> _people = FakeDatabase.People;

    private PersonService() { }

    public void AddPerson(Person person)
    {
        _people.Add(person);
    }

    public void RemovePerson(Person person)
    {
        _people.Remove(person);
    }

    public Person? GetPerson(Guid id)
    {
        return _people.FirstOrDefault(person => person.Id == id);
    }

    public IReadOnlyList<Person> GetList()
    {
        return _people.ToList();
    }

    public IReadOnlyList<Person> Search(string query)
    {
        return _people.Where(person => person.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }
}