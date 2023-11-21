
open System
open System.Diagnostics
open System.Runtime.Intrinsics
open System.Runtime.InteropServices
open BenchmarkDotNet.Running
open FSharp.Core
open BenchmarkDotNet.Attributes
open FSharp.NativeInterop

module Odin =

    let [<Literal>] DLLName = "Odin/Odin"
    type FloatPtr = nativeptr<float>
    [<DllImport(DLLName, CallingConvention=CallingConvention.Cdecl)>]
    extern void add_arrays(FloatPtr result, FloatPtr a, FloatPtr b, int32 length)


type Benchmarks () =

    let size = 1_024
    let a = Array.create size 1.0
    let b = Array.create size 2.0
    let fResult = Array.zeroCreate size
    let oResult = Array.zeroCreate size

    [<Benchmark>]
    member _.FSharp () =

        (a, b)
        ||> Array.iteri2 (fun i aValue bValue ->
            fResult[i] <- aValue + bValue)

        fResult

    [<Benchmark>]
    member _.FSharpAVX2 () =

        let ra = &MemoryMarshal.GetArrayDataReference<double> a
        let rb = &MemoryMarshal.GetArrayDataReference<double> b
        let res = &MemoryMarshal.GetArrayDataReference<double> fResult
        for i: unativeint in unativeint 0 .. unativeint Vector256<double>.Count .. a.Length - Vector256<double>.Count |> unativeint do
            (Vector256.LoadUnsafe(&ra, i) + Vector256.LoadUnsafe(&rb, i)).StoreUnsafe(&res, i)

        fResult

    [<Benchmark>]
    member _.Odin () =

        use aPtr = fixed a
        use bPtr = fixed b
        use resultPtr = fixed oResult

        Odin.add_arrays(resultPtr, aPtr, bPtr, a.Length)
        oResult


let test () =

    let b = Benchmarks()
    let fsharpResult = b.FSharp()
    // let fsharpResult = b.FSharpAVX2()
    let odinResult = b.Odin()
    printfn $"{fsharpResult = odinResult}"

// test()


let _ = BenchmarkRunner.Run<Benchmarks>()
