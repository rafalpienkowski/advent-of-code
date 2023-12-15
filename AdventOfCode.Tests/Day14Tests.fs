module ``day 14 tests``

open System.IO
open AdventOfCode.Day14
open Xunit
open FsUnit.Xunit

let sampleInput =
    @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#...."

let expectedTilt =
    @"OOOO.#.O..
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
    sampleInput |> toControlPanel |> tilt |> dump |> should equal expectedTilt

[<Fact>]
let ``Should calculate load for sample input control panel`` () =
    sampleInput
    |> toControlPanel
    |> tilt
    |> calculateLoad
    |> should equal 136

[<Fact>]
let ``Should calculate load for control panel`` () =
    File.ReadAllText("./Inputs/Day14.txt")
    |> toControlPanel
    |> tilt
    |> calculateLoad
    |> should equal 112048

let firstCycle =
    @".....#....
....#...O#
...OO##...
.OO#......
.....OOO#.
.O#...O#.#
....O#....
......OOOO
#...O###..
#..OO#...."

[<Fact>]
let ``Should cycle once`` () =
    sampleInput
    |> toControlPanel
    |> cycle 1
    |> dump
    |> should equal firstCycle

let secondCycle =
    @".....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#..OO###..
#.OOO#...O"

[<Fact>]
let ``Should cycle twice`` () =
    sampleInput
    |> toControlPanel
    |> cycle 2
    |> dump
    |> should equal secondCycle

let thirdCycle =
    @".....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#...O###.O
#.OOO#...O"

[<Fact>]
let ``Should cycle three times`` () =
    sampleInput
    |> toControlPanel
    |> cycle 3
    |> dump
    |> should equal thirdCycle
  

[<Fact>]
let ``Should cycle 1000000000 times and calculate load`` () =
    sampleInput
    |> toControlPanel
    |> cycle 1000000000
    |> calculateLoad
    |> should equal 64
    
[<Fact>]
let ``Should calculate load for control panel cycled`` () =
    File.ReadAllText("./Inputs/Day14.txt")
    |> toControlPanel
    |> tilt
    |> calculateLoad
    |> should equal 112048

[<Fact>]
let ``Should cycle 1000000000 times and calculate load for input`` () =
    File.ReadAllText("./Inputs/Day14.txt")
    |> toControlPanel
    |> cycle 1000000000
    |> calculateLoad
    |> should equal 105606
    
