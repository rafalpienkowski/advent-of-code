module ``day 4 should``

open Xunit
open System.IO
open FsUnit.Xunit
open AdventOfCode2022.Day4

let sampleInput = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8"

[<Fact>]
let ``calculate fully contained pairs for sample input`` () =
    sampleInput
    |> toSections
    |> findFullyContained
    |> List.length
    |> should equal 2
    
[<Fact>]
let ``calculate fully contained pairs for test input`` () =
    File.ReadAllText("./Inputs/Day4.txt")
    |> toSections
    |> findFullyContained
    |> List.length
    |> should equal 507
