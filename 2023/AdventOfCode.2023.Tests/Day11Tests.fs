module ``day 11 tests``

open System
open System.IO
open AdventOfCode.Day11
open Xunit
open FsUnit.Xunit

let sampleInput =
    @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#....."

[<Fact>]
let ``Should find shortest path for pair 2`` () =
    let expansions = sampleInput |> countExpansions 2
    
    findShortestPath2 { Id = 1; X = 0; Y = 3 } { Id = 7; X = 8; Y = 7 }  expansions
    |> should equal 15L
    
    findShortestPath2 { Id = 5; X = 5; Y = 1 } { Id = 9; X = 9; Y = 4 } expansions
    |> should equal 9L
    
    findShortestPath2 { Id = 3; X = 2; Y = 0 } { Id = 6; X = 6; Y = 9 }  expansions
    |> should equal 17L
    
    findShortestPath2 { Id = 8; X = 9; Y = 0 } { Id = 9; X = 9; Y = 4 } expansions
    |> should equal 5L

[<Theory>]
[<InlineData(2, 374L)>]
[<InlineData(10, 1030L)>]
[<InlineData(100, 8410L)>]
let ``Should find shortest paths for all galactics in sample with expansion 2`` (size: int) (expectedSumOfPaths: Int64) =
    let expansions = sampleInput |> countExpansions size
    
    sampleInput
    |> findEveryGalactic
    |> findPairs
    |> List.map (fun galacticPair -> findShortestPath2 (fst galacticPair) (snd galacticPair) expansions )
    |> List.sum
    |> should equal expectedSumOfPaths

[<Theory>]
[<InlineData(1_000_000, 10494813L)>]
[<InlineData(2, 10494813L)>]
let ``Should find shortest paths for all galactics in input 2`` (size: int) (expectedSumOfPaths: Int64) =
    let input = File.ReadAllText("./Inputs/Day11.txt")
    let expansions = input  |> countExpansions 2
    
    input
    |> findEveryGalactic
    |> findPairs
    |> List.map (fun galacticPair -> findShortestPath2 (fst galacticPair) (snd galacticPair) expansions)
    |> List.sum
    |> should equal expectedSumOfPaths

