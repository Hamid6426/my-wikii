// Lesson 13: Control Flow (../13_control_flow.md)
// if, else if, and switch
// Run: dotnet run 13-01_grade_calculator.cs

foreach (int score in new[] { 95, 82, 67, 45 })
{
    string grade;
    if (score >= 90) grade = "A";
    else if (score >= 80) grade = "B";
    else if (score >= 60) grade = "C";
    else grade = "F";

    Console.WriteLine($"{score} -> {grade}");
}

foreach (var day in new[] { DayOfWeek.Saturday, DayOfWeek.Monday, DayOfWeek.Friday })
{
    string kind = day switch
    {
        DayOfWeek.Saturday or DayOfWeek.Sunday => "weekend",
        DayOfWeek.Friday => "almost weekend",
        _ => "weekday"
    };
    Console.WriteLine($"{day}: {kind}");
}

int choice = 3;
switch (choice)
{
    case 1:
        Console.WriteLine("one");
        break;
    case 3:
        Console.WriteLine("three");
        break;
    default:
        Console.WriteLine("other");
        break;
}

// Expected output should be:
// 95 -> A
// 82 -> B
// 67 -> C
// 45 -> F
// Saturday: weekend
// Monday: weekday
// Friday: almost weekend
// three
