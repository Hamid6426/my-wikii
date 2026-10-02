# 60 - Publishing and Deployment

## Build vs Publish

| Command          | Output                                          | Use for                        |
| ---------------- | ----------------------------------------------- | ------------------------------ |
| `dotnet build`   | Files in `bin/` for running on your machine     | Development                    |
| `dotnet publish` | A clean folder with everything needed to deploy | Giving the app to someone else |

```bash
dotnet publish -c Release -o out
```

| Option       | Meaning                            |
| ------------ | ---------------------------------- |
| `-c Release` | Optimized build, not the Debug one |
| `-o out`     | Put the result in the folder `out` |
| `-r <rid>`   | Target operating system and CPU    |
| `--no-build` | Publish what is already built      |

---

## Framework-Dependent

The default. The output holds only your code. The computer that runs it must have the .NET runtime installed.

```bash
dotnet publish -c Release -o out
```

For a hello-world app the folder held five files and was about 108 KB:

```
App
App.deps.json
App.dll
App.pdb
App.runtimeconfig.json
```

Run it:

```bash
dotnet out/App.dll      # works on any OS with the runtime
./out/App               # the launcher for the OS you built on
```

| Good                          | Bad                                      |
| ----------------------------- | ---------------------------------------- |
| Tiny output                   | The target needs the right runtime       |
| Gets runtime security updates | Version mismatches cause start-up errors |

---

## Self-Contained

The output includes the .NET runtime, so the target needs nothing installed.

```bash
dotnet publish -c Release -r linux-x64 --self-contained -o out
```

Same hello-world app: 192 files and about 80 MB.

| Good                         | Bad                                    |
| ---------------------------- | -------------------------------------- |
| Runs on a clean machine      | Large                                  |
| You choose the exact runtime | You must rebuild to ship runtime fixes |

---

## Runtime Identifiers

`-r` takes a runtime identifier (RID): operating system plus CPU.

| RID              | Target                                      |
| ---------------- | ------------------------------------------- |
| `win-x64`        | Windows, 64-bit Intel/AMD                   |
| `win-arm64`      | Windows on ARM                              |
| `linux-x64`      | Linux, 64-bit Intel/AMD                     |
| `linux-arm64`    | Linux on ARM (Raspberry Pi 4+, ARM servers) |
| `linux-musl-x64` | Alpine Linux                                |
| `osx-arm64`      | macOS on Apple silicon                      |
| `osx-x64`        | macOS on Intel                              |

You can publish for another OS from your machine. A Linux machine can produce `win-x64` output without Windows.

---

## Single File

Pack everything into one executable.

```bash
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o out
```

Hello world: one file plus the `.pdb`, about 71 MB. The `.pdb` holds debug symbols and does not need to ship.

---

## Trimming

Remove the parts of the runtime and libraries your code never uses.

```bash
dotnet publish -c Release -r linux-x64 --self-contained \
  -p:PublishSingleFile=true -p:PublishTrimmed=true -o out
```

Hello world dropped from about 71 MB to about 15 MB.

Trimming cannot see code reached only through reflection or by name strings. It can remove it, and the app then fails at run time. Test the trimmed build, and watch the trim warnings during publish. See [Attributes and Reflection](52_attributes_and_reflection.md).

---

## ReadyToRun

Compile ahead of time into native code mixed with IL, so the app starts faster. The file is larger.

```bash
dotnet publish -c Release -r linux-x64 --self-contained \
  -p:PublishReadyToRun=true -o out
```

Hello world with trimming, single file, and ReadyToRun was about 19 MB. The app still carries the JIT and the runtime.

---

## Native AOT

Compile everything to a native program with no JIT and no runtime.

```bash
dotnet publish -c Release -r linux-x64 -p:PublishAot=true -o out
```

Hello world: about 4 MB (plus a `.dbg` symbols file), starts almost instantly, and needs no .NET on the target.

| Needs                                                                                                        | Limits                                             |
| ------------------------------------------------------------------------------------------------------------ | -------------------------------------------------- |
| A C compiler and linker on the build machine (`gcc` or `clang` on Linux, Visual Studio C++ tools on Windows) | No runtime code generation                         |
| One build per target OS and CPU                                                                              | Reflection is restricted                           |
|                                                                                                              | JSON needs source generation instead of reflection |
|                                                                                                              | Some libraries do not support it                   |

Use it for small command-line tools and fast-starting services.

---

## Size Summary

All numbers are from one hello-world app on Linux x64 (.NET 10) and will vary:

| Kind                                 | Size   |
| ------------------------------------ | ------ |
| Framework-dependent                  | 108 KB |
| Self-contained                       | 80 MB  |
| Self-contained, single file          | 71 MB  |
| Self-contained, single file, trimmed | 15 MB  |
| Trimmed, single file, ReadyToRun     | 19 MB  |
| Native AOT                           | 4 MB   |

---

## Put Settings in the Project File

Avoid long command lines by setting the options once:

```xml
<PropertyGroup>
  <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
  <SelfContained>true</SelfContained>
  <PublishSingleFile>true</PublishSingleFile>
  <PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>
```

Then `dotnet publish -c Release -o out` does the rest.

---

## Version Your App

```xml
<PropertyGroup>
  <Version>1.2.3</Version>
</PropertyGroup>
```

Use `MAJOR.MINOR.PATCH`: bump MAJOR for breaking changes, MINOR for new features, PATCH for fixes.

---

## Publish a Library as a NuGet Package

```bash
dotnet pack -c Release
```

This creates a `.nupkg` file, such as `bin/Release/Lib.1.0.0.nupkg`. Upload it with `dotnet nuget push` to nuget.org or a private feed. Set `<PackageId>`, `<Authors>`, `<Description>`, and `<Version>` in the `.csproj` first.

---

## Deploy

The output folder is the deliverable. How it gets to a machine is separate:

| Method                  | How                                                                                       |
| ----------------------- | ----------------------------------------------------------------------------------------- |
| Copy files              | `scp -r out/ user@server:/opt/app/` and run `./App`                                       |
| Run as a service        | A systemd unit on Linux, or a Windows Service                                             |
| Container               | Copy the output into an image and run it. See [Docker](../../wiki/containers/docker/README.md) |
| Cloud app services      | Upload the folder or a zip, or connect a git repository                                   |
| Share with other people | Zip a self-contained single file per OS                                                   |

A minimal systemd unit:

```ini
[Unit]
Description=My App

[Service]
WorkingDirectory=/opt/app
ExecStart=/opt/app/App
Restart=always
Environment=MY_API_KEY=value-from-a-secret-store

[Install]
WantedBy=multi-user.target
```

Never put real secrets in a file that goes into git. See [Security Basics](59_security_basics.md).

---

## Release Checklist

- Tests pass in a Release build (see [Testing](56_testing.md))
- Version number is bumped
- Built with `-c Release`
- No secrets, test data, or debug settings in the output
- Tried the output on a machine without your development tools
- If trimmed or AOT, ran the real scenarios, not only start-up
- Output folder or package is saved with its version number so you can roll back

---

## Gotchas

- `dotnet publish` defaults to Release for `net8.0` and newer targets, but older targets default to Debug. Pass `-c Release` so it is always explicit
- A framework-dependent app fails to start on a machine with the wrong runtime. The error names the missing version
- A self-contained build is tied to one OS and CPU. A `linux-x64` build does not run on Windows or on ARM
- Single file may still ship a `.pdb` next to it, and apps that use native libraries can need extra settings
- `bin/` and `obj/` are build folders. Deploy from the publish folder only
- Trimming and AOT warnings are real problems, not noise
