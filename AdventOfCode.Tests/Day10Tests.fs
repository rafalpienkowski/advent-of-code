module ``day 10 tests``

open System.IO
open AdventOfCode.Day10
open Xunit
open FsUnit.Xunit

let simpleMaze = @".....
.S-7.
.|.|.
.L-J.
....."

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
