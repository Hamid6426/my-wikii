// Lesson 08: Primitive Data Types (../08_primitive_data_types.md)
// Sizes and ranges of primitive types
// Run: dotnet run 08-01_primitive_ranges.cs

Console.WriteLine($"{"Type",-8}{"Bytes",6}  {"Min",26}  {"Max",26}");
Row("sbyte", sizeof(sbyte), sbyte.MinValue, sbyte.MaxValue);
Row("byte", sizeof(byte), byte.MinValue, byte.MaxValue);
Row("short", sizeof(short), short.MinValue, short.MaxValue);
Row("int", sizeof(int), int.MinValue, int.MaxValue);
Row("long", sizeof(long), long.MinValue, long.MaxValue);
Row("ulong", sizeof(ulong), ulong.MinValue, ulong.MaxValue);

Console.WriteLine();
Console.WriteLine($"float   {sizeof(float)} bytes, max {float.MaxValue}");
Console.WriteLine($"double  {sizeof(double)} bytes, max {double.MaxValue}");
Console.WriteLine($"decimal {sizeof(decimal)} bytes, max {decimal.MaxValue}");
Console.WriteLine($"char    {sizeof(char)} bytes, 'A' is {(int)'A'}");
Console.WriteLine($"bool    default is {default(bool)}");

static void Row(string type, int bytes, object min, object max)
    => Console.WriteLine($"{type,-8}{bytes,6}  {min,26:N0}  {max,26:N0}");

// Expected output should be:
// Type     Bytes                         Min                         Max
// sbyte        1                        -128                         127
// byte         1                           0                         255
// short        2                     -32,768                      32,767
// int          4              -2,147,483,648               2,147,483,647
// long         8  -9,223,372,036,854,775,808   9,223,372,036,854,775,807
// ulong        8                           0  18,446,744,073,709,551,615
//
// float   4 bytes, max 3.4028235E+38
// double  8 bytes, max 1.7976931348623157E+308
// decimal 16 bytes, max 79228162514264337593543950335
// char    2 bytes, 'A' is 65
// bool    default is False
