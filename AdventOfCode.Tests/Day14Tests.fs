module ``day 14 tests``

open System
open System.IO
open AdventOfCode.Day14
open Xunit
open FsUnit.Xunit

let sampleInput = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#...."

let expectedTilt = @"OOOO.#.O..
OO..#....#
OO..O##..O
O..#.OO...
........#.
..#....#.#
..O..#.O.O
..O.......
#....###..
#....#...."

[<Fact>]
let ``Should tilt sample input control panel`` () =
    sampleInput
    |> toControlPanel
    |> tilt
    |> dump
    |> should equal expectedTilt
    
    
[<Fact>]
let ``Should calculate load for sample input control panel`` () =
    sampleInput
    |> toControlPanel
    |> tilt
    |> calculateLoad
    |> should equal 136
    
[<Fact>]
let ``Should calculate load for control panel`` () =
    File.ReadAllText("./Inputs/Day14/input.txt")
    |> toControlPanel
    |> tilt
    |> calculateLoad
    |> should equal 112048
