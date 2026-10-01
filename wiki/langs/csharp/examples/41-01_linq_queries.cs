// Lesson 41: LINQ (../41_linq.md)
// LINQ: filter, project, group, join, aggregate
// Run: dotnet run 41-01_linq_queries.cs

var people = new List<Person>
{
    new("Alice", 30, "Lahore"), new("Bob", 25, "Karachi"), new("Carol", 35, "Lahore"),
    new("Dave", 41, "Karachi"), new("Eve", 29, "Islamabad")
};

var adults = people.Where(p => p.Age >= 30).OrderBy(p => p.Name).Select(p => p.Name);
Console.WriteLine($"30 or older: {string.Join(", ", adults)}");

Console.WriteLine($"Average age: {people.Average(p => p.Age):F1}");
Console.WriteLine($"Oldest: {people.MaxBy(p => p.Age)!.Name}");
Console.WriteLine($"Anyone from Islamabad? {people.Any(p => p.City == "Islamabad")}");
Console.WriteLine($"All adults? {people.All(p => p.Age >= 18)}");

foreach (var group in people.GroupBy(p => p.City).OrderBy(g => g.Key))
{
    Console.WriteLine($"{group.Key}: {group.Count()} people, names {string.Join("/", group.Select(p => p.Name))}");
}

var cities = new List<City> { new("Lahore", "Punjab"), new("Karachi", "Sindh"), new("Islamabad", "ICT") };
var joined = people.Join(cities, p => p.City, c => c.Name, (p, c) => $"{p.Name} lives in {c.Province}");
Console.WriteLine(string.Join("; ", joined));

var page = people.OrderBy(p => p.Name).Skip(1).Take(2).Select(p => p.Name);
Console.WriteLine($"Page 2 of size 2: {string.Join(", ", page)}");

var lookup = people.ToDictionary(p => p.Name, p => p.Age);
Console.WriteLine(lookup["Carol"]);

var query = from p in people
            where p.Age < 30
            orderby p.Age descending
            select $"{p.Name} ({p.Age})";
Console.WriteLine(string.Join(", ", query));

record Person(string Name, int Age, string City);
record City(string Name, string Province);

// Expected output should be:
// 30 or older: Alice, Carol, Dave
// Average age: 32.0
// Oldest: Dave
// Anyone from Islamabad? True
// All adults? True
// Islamabad: 1 people, names Eve
// Karachi: 2 people, names Bob/Dave
// Lahore: 2 people, names Alice/Carol
// Alice lives in Punjab; Bob lives in Sindh; Carol lives in Punjab; Dave lives in Sindh; Eve lives in ICT
// Page 2 of size 2: Bob, Carol
// 35
// Eve (29), Bob (25)
