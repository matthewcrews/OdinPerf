To run benchmark you first need to build the Odin `.dll`. To do this, open a terminal and navigate to the root of the project. Build the Odin code using:

`odin build Odin -build-mode:dll -o:aggressive`

This should procuce the necessary `Odin.dll` in the root of the project. You can now run the benchmark using:

`dotnet run -c Release`