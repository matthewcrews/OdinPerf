To run benchmark you first need to build the Odin `.dll`. To do this, open a terminal and navigate to the root of the project. Build the Odin code using:

`odin build Odin -build-mode:dll -o:aggressive`

This should procuce the necessary `Odin.dll` in the root of the project. You can now run the benchmark using:

`dotnet run -c Release`

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
| FSharp_Bench1 | 317.711 us | 2.0966 us | 1.7507 us |
| Odin_Bench1   |   8.208 us | 0.1168 us | 0.1092 us |
| FSharp_Bench2 | 309.631 us | 4.3325 us | 4.0527 us |
| Odin_Bench2   |  12.531 us | 0.2471 us | 0.2311 us |
| FSharp_Min    |  18.446 us | 0.2437 us | 0.2280 us |
| Odin_Min      |   4.299 us | 0.0361 us | 0.0338 us |
```