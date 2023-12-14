To run benchmark you first need to build `Odin.dll`. Make sure you have [Odin installed](https://odin-lang.org/docs/install/). Open a terminal and navigate to the root of the project. Build the Odin code using:

`odin build Odin -build-mode:dll -o:aggressive`

This should procuce the necessary `Odin.dll` in the root of the project. You can now run the benchmark using:

`dotnet run -c Release -arch: x64`

Results from my machine:

```
// * Summary *

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2134/22H2/2022Update/SunValley2)
13th Gen Intel Core i9-13900K, 1 CPU, 32 logical and 24 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method        | Mean       | Error     | StdDev    |
|-------------- |-----------:|----------:|----------:|
| FSharp_Bench1 | 291.169 us | 2.8295 us | 2.6467 us |
| Odin_Bench1   |   8.233 us | 0.0817 us | 0.0764 us |
| FSharp_Bench2 | 300.321 us | 2.6954 us | 2.3894 us |
| Odin_Bench2   |  12.352 us | 0.2092 us | 0.1956 us |
| FSharp_Min    |  18.401 us | 0.2386 us | 0.2232 us |
| Odin_Min      |   4.351 us | 0.0474 us | 0.0443 us |
```