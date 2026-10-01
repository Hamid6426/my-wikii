// Lesson 32: IDisposable (../32_idisposable.md)
// The full dispose pattern and SafeHandle
// Run: dotnet run 32-02_dispose_pattern.cs

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

// 1. The full pattern: managed and unmanaged resources
var holder = new BufferHolder(64);
holder.Write(0, 42);
Console.WriteLine($"Read back: {holder.Read(0)}");
holder.Dispose();
holder.Dispose();                           // safe to call again
try { holder.Read(0); }
catch (ObjectDisposedException) { Console.WriteLine("Use after dispose is rejected"); }

// 2. A SafeHandle owns the unmanaged resource, so you need no finalizer yourself
using (var memory = new NativeMemory(16))
{
    Marshal.WriteInt32(memory.DangerousGetHandle(), 7);
    Console.WriteLine($"Native value: {Marshal.ReadInt32(memory.DangerousGetHandle())}");
}
Console.WriteLine("Native memory released");

// 3. Framework types are SafeHandles already
string path = Path.GetTempFileName();
using (SafeFileHandle file = File.OpenHandle(path))
{
    Console.WriteLine($"File handle valid: {!file.IsInvalid}");
}
File.Delete(path);

class BufferHolder : IDisposable
{
    private IntPtr _memory;                 // unmanaged resource
    private readonly List<string> _log = []; // managed resource
    private bool _disposed;

    public BufferHolder(int size) => _memory = Marshal.AllocHGlobal(size);

    public void Write(int offset, int value)
    {
        ThrowIfDisposed();
        Marshal.WriteInt32(_memory, offset, value);
    }

    public int Read(int offset)
    {
        ThrowIfDisposed();
        return Marshal.ReadInt32(_memory, offset);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);          // no need for the finalizer now
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _log.Clear();                   // managed: only touch these when called from Dispose()
        }

        Marshal.FreeHGlobal(_memory);       // unmanaged: always release
        _memory = IntPtr.Zero;
        _disposed = true;
    }

    ~BufferHolder() => Dispose(disposing: false);   // safety net if Dispose() was forgotten

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);
}

class NativeMemory : SafeHandleZeroOrMinusOneIsInvalid
{
    public NativeMemory(int size) : base(ownsHandle: true) => SetHandle(Marshal.AllocHGlobal(size));

    protected override bool ReleaseHandle()
    {
        Marshal.FreeHGlobal(handle);
        return true;
    }
}

// Expected output should be:
// Read back: 42
// Use after dispose is rejected
// Native value: 7
// Native memory released
// File handle valid: True
