using System;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Sorting", (List<string> names) =>
{
    if (!names.Any()) return Results.NoContent();

    //Create Students
    List<Student> students = names.Select(a => new Student(a)).ToList();

    //Sorting
    foreach (var s in students)
    {
        Random random = new Random();
        s.House = Enum.GetName(typeof(Houses), random.Next(1, 5));
    }

    //Create House and put student in it
    List<House> houses = new();
    foreach (var h in students.Select(a => a.House).Distinct().ToList())
    {
        House house = new(h) { Students = students.Where(a => a.House == h).OrderBy(a => a.Name).ToList() };
        houses.Add(house);
    }

    //Spreading
    var avg = (int)Math.Ceiling(names.Count / 4.0);
    houses = houses.OrderByDescending(a => a.NumberStudents).ToList();

    List<Student> centers = new();

    //Get student from house that there are number of student more than avg
    houses.ForEach(h =>
    {
        var Num = h.NumberStudents - avg;
        if (Num > 0)
        {
            var list = h.Students.Take(Num).ToList();
            centers.AddRange(list);
            h.Students.RemoveAll(a => list.Select(b => b.Id).Contains(a.Id));
        }
    });

    houses.Where(a => a.NumberStudents < avg).ToList().ForEach(h =>
          {
              var num = avg - 1 - h.NumberStudents;
              var s = centers.Take(num).ToList();
              h.Students.AddRange(s);
              centers.RemoveAll(a => s.Select(b => b.Id).Contains(a.Id));
          });

    //Re-Naming
    houses.ForEach(h => h.Students.ForEach(s => s.House = h.Name));

    //Result
    return Results.Ok(houses);
});

app.Run();


record Student(string Name)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName = Name;
    public string House { get; set; } = string.Empty;
}

record House(string Name)
{
    public string HouseName = Name;
    public int NumberStudents { get { return Students.Count; } }
    public List<Student> Students { get; set; } = new();
}

enum Houses
{
    Gryffindor = 1,
    Hufflepuff,
    Ravenclaw,
    Slytherin
}