module ``day 13 tests``

open System.IO
open AdventOfCode.Day13
open Xunit
open FsUnit.Xunit

let rowSample = @"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#"

[<Fact>]
let ``Should find reflection in sample input`` () =
    rowSample
    |> findPerfectReflection 0
    |> summarize
    |> should equal 405

let secondSample = @".......#.####
#..#.##.#####
.#...#..#....
#..#.###.....
.##.#..##.##.
#####.#..####
....######..#"

[<Fact>]
let ``Should find reflection in second sample`` () =
    secondSample
    |> findPerfectReflection 0
    |> summarize
    |> should equal 11
    
[<Fact>]
let ``Should find reflection in input`` () =
    File.ReadAllText("./Inputs/Day13/input.txt")
    |> findPerfectReflection 0
    |> summarize
    |> should equal 32723
