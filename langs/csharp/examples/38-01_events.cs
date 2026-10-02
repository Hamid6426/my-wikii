// Lesson 38: Events (../38_events.md)
// Events with EventHandler
// Run: dotnet run 38-01_events.cs

var thermostat = new Thermostat();

thermostat.TemperatureChanged += (sender, e) =>
    Console.WriteLine($"Temperature changed: {e.Old} -> {e.New}");

thermostat.TemperatureChanged += OnHot;

thermostat.Set(20);
thermostat.Set(20);   // same value: no event
thermostat.Set(31);

thermostat.TemperatureChanged -= OnHot;
thermostat.Set(35);

static void OnHot(object? sender, TemperatureEventArgs e)
{
    if (e.New > 30) Console.WriteLine("  Warning: it is hot");
}

class TemperatureEventArgs(int oldValue, int newValue) : EventArgs
{
    public int Old { get; } = oldValue;
    public int New { get; } = newValue;
}

class Thermostat
{
    private int _temperature;

    public event EventHandler<TemperatureEventArgs>? TemperatureChanged;

    public void Set(int value)
    {
        if (value == _temperature) return;
        var args = new TemperatureEventArgs(_temperature, value);
        _temperature = value;
        TemperatureChanged?.Invoke(this, args);
    }
}

// Expected output should be:
// Temperature changed: 0 -> 20
// Temperature changed: 20 -> 31
//   Warning: it is hot
// Temperature changed: 31 -> 35
