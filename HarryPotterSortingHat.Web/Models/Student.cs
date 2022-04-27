using System;
namespace HarryPotterSortingHat.Web.Models;

public class Student
{
    public Student()
    {

    }

    public Student(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string House { get; set; } = string.Empty;
}