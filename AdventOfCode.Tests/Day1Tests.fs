module ``day 1 tests``

open System.IO
open AdventOfCode
open Xunit
open Day1
open FsUnit.Xunit

let sampleInput = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet"

let sampleSpelledInput = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen"

[<Fact>]
let ``Should calculate 142 for sample input`` () =
    calculateCalibrationValue sampleInput
    |> should equal 142

[<Fact>]
let ``Should calculate 281 for sample spelled input`` () =
    calculateCalibrationValue sampleSpelledInput
    |> should equal 281
    
[<Fact>]
let ``Should calculate result for day 1`` () =
    File.ReadAllText("./Inputs/Day1.txt")
    |> calculateCalibrationValue
    |> should equal 54249
