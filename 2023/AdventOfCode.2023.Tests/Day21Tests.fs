module ``day 21 tests``

open System
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

[<Theory>]
[<InlineData(6L, 16)>]
[<InlineData(10L, 50)>]
[<InlineData(50L, 1594)>]
[<InlineData(100L, 6536)>]
[<InlineData(500L, 167004)>]
[<InlineData(1000L, 668697)>]
[<InlineData(5000L, 16733044)>]
let ``Should take n steps for sample garden`` (steps: Int64) (plots: int) =
    sampleInput
    |> parseGarden
    |> findGardenPlots steps
    |> List.length
    |> should equal plots

[<Fact>]
let ``Should take 64 steps for test garden`` () =
    File.ReadAllText("./Inputs/Day21.txt")
    |> parseGarden
    |> findGardenPlots 64
    |> List.length
    |> should equal 3617
