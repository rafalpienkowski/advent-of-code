module ``day 10 tests``

open System.IO
open AdventOfCode.Day10
open Xunit
open FsUnit.Xunit

let simpleMaze = @"......
..S-7.
..|.|.
..L-J.
......"

[<Fact>]
let ``Should count the longest pipe length for simple maze`` () =
    simpleMaze
    |> countLongestLength
    |> should equal 4
    
let moreComplexSample = @"..F7.
.FJ|.
SJ.L7
|F--J
LJ..."

[<Fact>]
let ``Should count the longest pipe length for more complex sample`` () =
    moreComplexSample
    |> countLongestLength
    |> should equal 8
    
[<Fact>]
let ``Should count the longest pipe length for input`` () =
    File.ReadAllText("./Inputs/Day10/input.txt")
    |> countLongestLength
    |> should equal 6773

let simpleLoop = @"............
..S-------7.
..|F-----7|.
..||.....||.
..||.....||.
..|L-7.F-J|.
..|..|.|..|.
..L--J.L--J.
............"

let ``Should calculate nest count in simple loop`` () =
    simpleLoop
    |> calculateNestCount
    |> should equal 4
    
let simpleLoop2 = @"..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
.........."
    
let ``Should calculate nest count in simple loop 2`` () =
    simpleLoop2
    |> calculateNestCount
    |> should equal 4

let complexLoop = @".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ..."

let ``Should calculate nest count in complex loop `` () =
    complexLoop
    |> calculateNestCount
    |> should equal 8
