module ``day 2 should``

open Xunit
open System.IO
open FsUnit.Xunit
open AdventOfCode2022.Day2

let sampleInput = @"A Y
B X
C Z"

[<Fact>]
let ``calculate calculate total score from sample input`` () =
    sampleInput
    |> loadStrategy
    |> calculateSum
    |> should equal 15
    
[<Fact>]
let ``calculate calculate total score from test input`` () =
    File.ReadAllText("./Inputs/Day2.txt")
    |> loadStrategy
    |> calculateSum
    |> should equal 13484

[<Fact>]
let ``calculate calculate strategy score from sample input`` () =
    sampleInput
    |> loadGuides
    |> calculateStrategy
    |> should equal 12
    
[<Fact>]
let ``calculate calculate strategy score from test input`` () =
    File.ReadAllText("./Inputs/Day2.txt")
    |> loadGuides
    |> calculateStrategy
    |> should equal 13433
