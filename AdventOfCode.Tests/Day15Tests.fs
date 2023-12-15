module ``day 15 tests``

open System
open System.IO
open AdventOfCode.Day15
open Xunit
open FsUnit.Xunit

let hashSample = "HASH"

[<Fact>]
let ``Should calculate hash for hash input`` () =
    hashSample
    |> hash
    |> should equal 52

let sampleInput = @"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"

[<Fact>]
let ``Should calculate hash for sample input`` () =
    sampleInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map hash
    |> Array.sum
    |> should equal 1320
    
[<Fact>]
let ``Should calculate hash for test input`` () =
    File.ReadAllText("./Inputs/Day15/input.txt").Split(',', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map hash
    |> Array.sum
    |> should equal 521341
