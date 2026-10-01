// Lesson 23: Reference Data Types (../23_reference_data_types.md)
// Value types vs reference types, boxing
// Run: dotnet run 23-01_value_vs_reference.cs

var p1 = new PointClass { X = 1 };
var p2 = p1;                 // copies the reference
p2.X = 99;
Console.WriteLine($"class:  p1.X = {p1.X} (shared)");

var s1 = new PointStruct { X = 1 };
var s2 = s1;                 // copies the value
s2.X = 99;
Console.WriteLine($"struct: s1.X = {s1.X} (independent)");

string a = "hello";
string b = a;
b += " world";
Console.WriteLine($"{a} / {b}");

int number = 42;
object boxed = number;       // boxing
int unboxed = (int)boxed;    // unboxing
Console.WriteLine($"{boxed} {unboxed}");

Console.WriteLine(ReferenceEquals(p1, p2));
Console.WriteLine(new PointStruct { X = 5 }.Equals(new PointStruct { X = 5 }));

class PointClass { public int X; }
struct PointStruct { public int X; }

// Expected output should be:
// class:  p1.X = 99 (shared)
// struct: s1.X = 1 (independent)
// hello / hello world
// 42 42
// True
// True
