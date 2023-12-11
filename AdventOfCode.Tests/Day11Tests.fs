module ``day 11 tests``

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
let ``Should add expand galactic`` () =

    let expectedGalactic =
        @"....#........
.........#...
#............
.............
.............
........#....
.#...........
............#
.............
.............
.........#...
#....#......."

    sampleInput |> expandGalactic |> should equal expectedGalactic


let sampleGalactic =
    [ { Id = 1; X = 0; Y = 4 }
      { Id = 2; X = 1; Y = 9 }
      { Id = 3; X = 2; Y = 0 }
      { Id = 4; X = 5; Y = 8 }
      { Id = 5; X = 6; Y = 1 }
      { Id = 6; X = 7; Y = 12 }
      { Id = 7; X = 10; Y = 9 }
      { Id = 8; X = 11; Y = 0 }
      { Id = 9; X = 11; Y = 5 } ]
    
[<Fact>]
let ``Should find every galactic`` () =
    sampleInput
    |> expandGalactic
    |> findEveryGalactic
    |> should equal sampleGalactic

[<Fact>]
let ``Should find all pairs`` () =
    sampleGalactic
    |> findPairs
    |> List.length
    |> should equal 36
    
[<Fact>]
let ``Should find shortest path for pair`` () =
    findShortestPath { Id = 5; X = 6; Y = 1 } { Id = 9; X = 11; Y = 5 }
    |> should equal 9
    
    findShortestPath { Id = 1; X = 0; Y = 4 } { Id = 7; X = 10; Y = 9 }
    |> should equal 15
    
    findShortestPath { Id = 3; X = 2; Y = 0 } { Id = 6; X = 7; Y = 12 }
    |> should equal 17
    
    findShortestPath { Id = 8; X = 11; Y = 0 } { Id = 9; X = 11; Y = 5 }
    |> should equal 5

[<Fact>]
let ``Should find shortest paths for all galactics in sample`` () =
    sampleInput
    |> expandGalactic
    |> findEveryGalactic
    |> findPairs
    |> List.map (fun galacticPair -> findShortestPath (fst galacticPair) (snd galacticPair) )
    |> List.sum
    |> should equal 374

[<Fact>]
let ``Should find shortest paths for all galactics in input`` () =
    File.ReadAllText("./Inputs/Day11/input.txt")
    |> expandGalactic
    |> findEveryGalactic
    |> findPairs
    |> List.map (fun galacticPair -> findShortestPath (fst galacticPair) (snd galacticPair) )
    |> List.sum
    |> should equal 374
