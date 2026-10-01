// Lesson 07: Variables & Constants (../07_variable_and_constants.md)
// Variables, var, const, readonly
// Run: dotnet run 07-01_variables_and_constants.cs

string name = "Alice";
int age = 30;
var city = "Lahore";          // type inferred as string
const double Pi = 3.14159;    // compile-time constant
int x = 1, y = 2, z = 3;

Console.WriteLine($"{name}, {age}, {city}");
Console.WriteLine($"{x + y + z}");
Console.WriteLine(Pi * 2);

var settings = new Settings("prod");
Console.WriteLine(settings.Environment);

(x, y) = (y, x);              // swap
Console.WriteLine($"x={x}, y={y}");

class Settings(string env)
{
    public readonly string Environment = env;   // set once, in the constructor
}

// Expected output should be:
// Alice, 30, Lahore
// 6
// 6.28318
// prod
// x=2, y=1
