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
    |> loadStrategyGuide
    |> calculateSum
    |> should equal 15
    
    
[<Fact>]
let ``calculate calculate total score from test input`` () =
    File.ReadAllText("./Inputs/Day2.txt")
    |> loadStrategyGuide
    |> calculateSum
    |> should equal 13484
