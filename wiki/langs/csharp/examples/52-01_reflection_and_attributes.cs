#:property PublishAot=false
// Lesson 52: Attributes and Reflection (../52_attributes_and_reflection.md)
// Custom attributes and reflection
// Run: dotnet run 52-01_reflection_and_attributes.cs

using System.Reflection;

var user = new User { Name = "Alice", Email = "alice@example.com", Age = 17 };
Console.WriteLine(string.Join("; ", Validate(user)));

user.Name = "";
user.Age = 30;
Console.WriteLine(string.Join("; ", Validate(user)));

Console.WriteLine("--- properties ---");
foreach (PropertyInfo p in typeof(User).GetProperties())
{
    Console.WriteLine($"{p.Name}: {p.PropertyType.Name}, attributes: {p.GetCustomAttributes().Count()}");
}

object? created = Activator.CreateInstance(typeof(User));
typeof(User).GetProperty("Name")!.SetValue(created, "Created by reflection");
Console.WriteLine(((User)created!).Name);

MethodInfo upper = typeof(string).GetMethod("ToUpper", Type.EmptyTypes)!;
Console.WriteLine(upper.Invoke("shout", null));

static List<string> Validate(object obj)
{
    var errors = new List<string>();
    foreach (PropertyInfo prop in obj.GetType().GetProperties())
    {
        object? value = prop.GetValue(obj);

        if (prop.GetCustomAttribute<RequiredTextAttribute>() != null && string.IsNullOrWhiteSpace(value as string))
            errors.Add($"{prop.Name} is required");

        var range = prop.GetCustomAttribute<RangeAttribute>();
        if (range != null && value is int n && (n < range.Min || n > range.Max))
            errors.Add($"{prop.Name} must be {range.Min} to {range.Max}");
    }
    return errors.Count == 0 ? ["valid"] : errors;
}

[AttributeUsage(AttributeTargets.Property)]
class RequiredTextAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
class RangeAttribute(int min, int max) : Attribute
{
    public int Min { get; } = min;
    public int Max { get; } = max;
}

class User
{
    [RequiredText] public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    [Range(18, 120)] public int Age { get; set; }
}

// Expected output should be:
// Age must be 18 to 120
// Name is required
// --- properties ---
// Name: String, attributes: 1
// Email: String, attributes: 0
// Age: Int32, attributes: 1
// Created by reflection
// SHOUT
