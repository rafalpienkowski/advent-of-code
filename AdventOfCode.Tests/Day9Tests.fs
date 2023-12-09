module ``day 9 tests``

open System
open System.IO
open AdventOfCode.Day9
open Xunit
open FsUnit.Xunit

[<Theory>]
[<InlineData("0 3 6 9 12 15", 18L)>]
[<InlineData("1 3 6 10 15 21", 28L)>]
[<InlineData("10 13 16 21 30 45", 68L)>]
let ``Should calculate next value`` (line: string) (expectedValue: Int64) =
    line |> predictNextValue |> should equal expectedValue

[<Fact>]
let ``Should sum prediction from input file`` () =
    File
        .ReadAllText("./Inputs/Day9/input.txt")
        .Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map predictNextValue
    |> List.sum
    |> should equal 1938800261L

[<Theory>]
[<InlineData("0 3 6 9 12 15", -3L)>]
[<InlineData("1 3 6 10 15 21", 0L)>]
[<InlineData("10 13 16 21 30 45", 5L)>]
let ``Should calculate previous value`` (line: string) (expectedValue: Int64) =
    line |> predictPreviousValue |> should equal expectedValue

[<Fact>]
let ``Should sum history from input file`` () =
    File
        .ReadAllText("./Inputs/Day9/input.txt")
        .Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map predictPreviousValue
    |> List.sum
    |> should equal 1112L
