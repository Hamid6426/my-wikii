// Lesson 05: Console Input and Output (../05_console_input_and_output.md)
// Format numbers and align columns
// Run: dotnet run 05-02_format_table.cs

var items = new[]
{
    (Name: "Notebook", Qty: 3, Price: 4.5),
    (Name: "Pen", Qty: 12, Price: 0.75),
    (Name: "Backpack", Qty: 1, Price: 39.9)
};

Console.WriteLine($"{"Item",-10}{"Qty",5}{"Price",10}{"Total",10}");
Console.WriteLine(new string('-', 35));

double sum = 0;
foreach (var (name, qty, price) in items)
{
    double total = qty * price;
    sum += total;
    Console.WriteLine($"{name,-10}{qty,5}{price,10:N2}{total,10:N2}");
}

Console.WriteLine(new string('-', 35));
Console.WriteLine($"{"Sum",-10}{"",5}{"",10}{sum,10:N2}");
Console.WriteLine($"ID {7:D4}, hex {255:X}, percent {0.256:P1}");

// Expected output should be:
// Item        Qty     Price     Total
// -----------------------------------
// Notebook      3      4.50     13.50
// Pen          12      0.75      9.00
// Backpack      1     39.90     39.90
// -----------------------------------
// Sum                           62.40
// ID 0007, hex FF, percent 25.6%
