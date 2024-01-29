using COP4870_LMS.Models;

namespace COP4870_LMS.Services;

public class CourseService
{
    private readonly IList<Course> _courses;
    
    public CourseService()
    {
        _courses = new List<Course>();
    }

    public CourseService(IList<Course> courses)
    {
        _courses = courses;
    }

    public void AddCourse(Course course)
    {
        _courses.Add(course);
    }

    public void RemoveCourse(Course course)
    {
        _courses.Remove(course);
    }

    public Course? GetCourse(Guid id)
    {
        return _courses.FirstOrDefault(course => course.Id == id);
    }

    public IReadOnlyList<Course> GetList()
    {
        return _courses.ToList();
    }

    public IReadOnlyList<Course> Search(string query)
    {
        query = query.ToLower();
        return _courses.Where(course => 
            course.Name.ToLower().Contains(query) || course.Description.ToLower().Contains(query)
        ).ToList();
    }
    
    public IReadOnlyList<Course> FilterByPerson(Guid personId)
    {
        return _courses.Where(course => course.Roster.Contains(personId)).ToList();
    }
}