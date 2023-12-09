module ``day 8 tests``

open System.IO
open AdventOfCode.Day8
open Xunit
open FsUnit.Xunit

let sampleNetwork = @"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)"

let anotherSampleNetwork = $"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)"

[<Fact>]
let ``Should parse sample network`` () =
    parse sampleNetwork
    |> should equal { Navigation = "LLR"
                      Nodes = [
                          { Location = "AAA"; Left = "BBB"; Right = "BBB"; EndsWithZ = false }
                          { Location = "BBB"; Left = "AAA"; Right = "ZZZ"; EndsWithZ = false }
                          { Location = "ZZZ"; Left = "ZZZ"; Right = "ZZZ"; EndsWithZ = true }
                      ] }
    
[<Fact>]
let ``Should count steps for sampleNetwork`` () =
    sampleNetwork
    |> parse
    |> countStepsToZZZ
    |> should equal 6

[<Fact>]
let ``Should count steps for anotherSampleNetwork`` () =
    anotherSampleNetwork
    |> parse
    |> countStepsToZZZ
    |> should equal 2

[<Fact>]
let ``Should count steps for input`` () =
    File.ReadAllText("./Inputs/Day8/input.txt")
    |> parse
    |> countStepsToZZZ
    |> should equal 18023

let ghostSample = @"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)"

[<Fact>]
let ``Should count ghost steps from ghost sample`` () =
    ghostSample
    |> parse
    |> countGhostSteps
    |> should equal 6L
    
[<Fact>]
let ``Should count ghost steps for input`` () =
    File.ReadAllText("./Inputs/Day8/input.txt")
    |> parse
    |> countGhostSteps
    |> should equal 14449445933179L
