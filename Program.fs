
open System
open System.Diagnostics
open System.Runtime.InteropServices
open BenchmarkDotNet.Running
open FSharp.Core
open BenchmarkDotNet.Attributes
open FSharp.NativeInterop

#nowarn "9"

module Odin =

    open System.Runtime.InteropServices
    open Microsoft.FSharp.NativeInterop

    type Slice<'a when 'a : unmanaged> =
        struct
           val Pointer: nativeptr<'a>
           val Length: nativeint

           new (ptr, l) =
               {
                   Pointer = ptr
                   Length = nativeint l
               }

           member s.Item
                with get (i: int) =
                    NativePtr.get s.Pointer i
                and set i v =
                    NativePtr.set s.Pointer i v
        end

    module Slice =

        let create<'a when 'a: unmanaged>(arr: 'a[]) =
            use ptr = fixed arr
            Slice (ptr, arr.Length)

    let [<Literal>] DLLName = "Odin"
    type IntPtr = nativeptr<int>

    [<DllImport(DLLName, CallingConvention=CallingConvention.Cdecl)>]
    extern void bench1(IntPtr result, IntPtr a, IntPtr b, int32 length)

    [<DllImport(DLLName, CallingConvention=CallingConvention.Cdecl)>]
    extern void bench2(IntPtr result, IntPtr a, IntPtr b, int32 length)

    [<DllImport(DLLName, CallingConvention=CallingConvention.Cdecl)>]
    extern void bench3(IntPtr result, IntPtr a, IntPtr b, int32 length)

    type IntSlice = Slice<int>
    [<DllImport(DLLName, CallingConvention=CallingConvention.Cdecl)>]
    extern void min(IntSlice a)


type Benchmarks () =

    let rng = Random 123
    let size = 100_000
    let a = Array.init size (fun _ ->
        rng.Next())
    let b = Array.init size (fun _ ->
        rng.Next())
    let result = Array.zeroCreate size
    let oResult = Array.zeroCreate size

    [<Benchmark>]
    member _.FSharp_Bench1 () =

        (a, b)
        ||> Array.iteri2 (fun i aValue bValue ->
            if aValue < bValue then
                result[i] <- aValue + bValue
            else
                result[i] <- aValue - bValue)

        result

    [<Benchmark>]
    member _.Odin_Bench1 () =

        use aPtr = fixed a
        use bPtr = fixed b
        use resultPtr = fixed oResult

        Odin.bench1(resultPtr, aPtr, bPtr, a.Length)
        oResult

    [<Benchmark>]
    member _.FSharp_Bench2 () =

        (a, b)
        ||> Array.iteri2 (fun i aValue bValue ->
            if aValue < bValue then
                result[i] <- aValue + bValue * aValue
            else
                result[i] <- aValue * bValue - bValue)

        result

    [<Benchmark>]
    member _.Odin_Bench2 () =

        use aPtr = fixed a
        use bPtr = fixed b
        use resultPtr = fixed oResult

        Odin.bench2(resultPtr, aPtr, bPtr, a.Length)
        oResult

    [<Benchmark>]
    member _.FSharp_Min () =
        Array.min a

    [<Benchmark>]
    member _.Odin_Min () =

        let aSlice = Odin.Slice.create a

        Odin.min(aSlice)

    // [<Benchmark>]
    // member _.Odin_Bench3 () =
    //
    //     use aPtr = fixed a
    //     use bPtr = fixed b
    //     use resultPtr = fixed oResult
    //
    //     Odin.bench3(resultPtr, aPtr, bPtr, a.Length)
    //     oResult



let test () =

    let b = Benchmarks()
    let fsharpResult = b.FSharp_Bench2()
    let odinResult = b.Odin_Bench2()
    printfn $"{fsharpResult = odinResult}"

// test()


let _ = BenchmarkRunner.Run<Benchmarks>()
