using Lms.Library.Models;

namespace Lms.Library.Services;

public class PersonService
{
    private readonly IList<Person> _people;

    public PersonService()
    {
        _people = new List<Person>();
    }

    public PersonService(IList<Person> people)
    {
        _people = people;
    }

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
        return _people.Where(person => person.Name.ToLower().Contains(query.ToLower())).ToList();
    }
}