using System;
namespace HarryPotterSortingHat.Web.Models;

public class House
{
    public House(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public int NumberStudents { get { return Students.Count; } }
    public List<Student> Students { get; set; } = new();
}