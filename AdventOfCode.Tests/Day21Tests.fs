module ``day 21 tests``

open System.IO
open AdventOfCode.Day21
open Xunit
open FsUnit.Xunit

let sampleInput = @"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
..........."

[<Fact>]
let ``Should take 6 steps for sample garden`` () =
    sampleInput
    |> parseGarden
    |> findGardenPlots 6
    |> List.length
    |> should equal 16
    

[<Fact>]
let ``Should take 64 steps for test garden`` () =
    File.ReadAllText("./Inputs/Day21.txt")
    |> parseGarden
    |> findGardenPlots 64
    |> List.length
    |> should equal 3617
