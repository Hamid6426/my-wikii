// Lesson 28: Indexers and Operator Overloading (../28_indexers_and_operator_overloading.md)
// Indexers and operator overloading
// Run: dotnet run 28-01_indexers_and_operators.cs

var matrix = new Matrix2x2(1, 2, 3, 4);
Console.WriteLine(matrix[0, 1]);
Console.WriteLine(matrix + matrix);
Console.WriteLine(matrix * 3);

var v1 = new Vector(1, 2);
var v2 = new Vector(3, 4);
Console.WriteLine(v1 + v2);
Console.WriteLine(v2 - v1);
Console.WriteLine(v1 == new Vector(1, 2));
Console.WriteLine(-v1);

var week = new WeekDays();
Console.WriteLine(week[2]);
Console.WriteLine(week["fri"]);

record Matrix2x2(int A, int B, int C, int D)
{
    public int this[int row, int col] => (row, col) switch
    {
        (0, 0) => A, (0, 1) => B, (1, 0) => C, (1, 1) => D,
        _ => throw new IndexOutOfRangeException()
    };

    public static Matrix2x2 operator +(Matrix2x2 m, Matrix2x2 n) => new(m.A + n.A, m.B + n.B, m.C + n.C, m.D + n.D);
    public static Matrix2x2 operator *(Matrix2x2 m, int k) => new(m.A * k, m.B * k, m.C * k, m.D * k);
}

readonly struct Vector(double x, double y) : IEquatable<Vector>
{
    public double X { get; } = x;
    public double Y { get; } = y;

    public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector operator -(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector operator -(Vector a) => new(-a.X, -a.Y);
    public static bool operator ==(Vector a, Vector b) => a.Equals(b);
    public static bool operator !=(Vector a, Vector b) => !a.Equals(b);

    public bool Equals(Vector other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Vector v && Equals(v);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";
}

class WeekDays
{
    private readonly string[] _days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

    public string this[int index] => _days[index];
    public int this[string name] => Array.FindIndex(_days, d => d.Equals(name, StringComparison.OrdinalIgnoreCase));
}

// Expected output should be:
// 2
// Matrix2x2 { A = 2, B = 4, C = 6, D = 8 }
// Matrix2x2 { A = 3, B = 6, C = 9, D = 12 }
// (4, 6)
// (2, 2)
// True
// (-1, -2)
// Wed
// 4
