# Java Setup

## Search & Download

- Search and find java from trusted source e.g. I choose oracle:

[https://www.oracle.com/java/technologies/downloads](https://www.oracle.com/java/technologies/downloads)

- Select the LTS version tab as mentioned at the start (I chose JDK 25)
- Select the operating system (I chose Windows)
- Download any of the 3 versions (I chose MSI)
- Download the SHA256 key if concerned about the security i.e.

```
03dce75d871ddf5029bdedbcd09787e8bac63f48872e03ad870176c72c44cecd
```

## Verification

- Open powershell
- Go to the download folder

```ps1
cd ~\Downloads
```

```ps1
Get-FileHash .\jdk-25_windows-x64_bin.msi -Algorithm SHA256
```

- Compare the output from the command above with SHA256 key
- The key is not case sensitive 1-1 i.e. A = a
- If both matches then it can be considered as the correct download

## Installation

- Choose a directory (Better to select default as AI can help with it if anything occurs)
- Install java and close the insallater afterward.
- Verify with the following command

```ps1
java --version
```

- Check after closing the powershell/cmd and launching again.
- Expected result

```ps1
java 25.0.2 2026-01-20 LTS
Java(TM) SE Runtime Environment (build 25.0.2+10-LTS-69)
Java HotSpot(TM) 64-Bit Server VM (build 25.0.2+10-LTS-69, mixed mode, sharing)
```

## Environment Variable Setup

### JAVA_HOME setup

This is not necassary but really helpful like setting environment variables ensures that the Java compiler (`javac - Compile .java files`) and the Java Runtime Environment (`java - Run compiled java files/classes`) are accessible from any terminal or command prompt on your system.

- Copy the installation directory to clipboard

```
C:\Program Files\Java\jdk-25.0.2
```

- Press the `Windows Key`, type "Environment Variables", and select `Edit the system environment variables`.
- In the System Properties window, click `Environment Variables`.
- Under System variables, click `New`.
- Fill the form with the following:

Variable name: `JAVA_HOME`
Variable value: `C:\Program Files\Java\jdk-25.0.2`

- Click `OK`.

### Update the Path Variable

This allows you to run Java commands without typing the full directory path.

- In the same `Environment Variables` window, find the `Path` variable under `System variables` and click `Edit`.
- Click `New` and type: `%JAVA_HOME%\bin`
- Click `OK` on all open windows to save the changes.

### Verify

- Verify with this command from powershell/cmd

```
echo $env:JAVA_HOME
```

- Expected result must be:

```
C:\Program Files\Java\jdk-25.0.2
```

This is the end of java setup, commands like `javac` and `java` which compile and run classes respectively will be described and used in the next part of the docs.
