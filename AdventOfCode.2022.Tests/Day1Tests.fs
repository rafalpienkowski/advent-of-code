module ``day 1 should``

open System
open Xunit
open System.IO
open Xunit
open FsUnit.Xunit
open AdventOfCode2022.Day1

let sampleInput = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000"

[<Fact>]
let ``find elf with the most calories from sample input`` () =
    sampleInput
    |> parse
    |> List.maxBy ( fun e -> e.Calories |> List.sum)
    |> sumCalories
    |> should equal 24000
    
[<Fact>]
let ``find elf with the most calories from test input`` () =
    File.ReadAllText("./Inputs/Day1.txt")
    |> parse
    |> List.maxBy ( fun e -> e.Calories |> List.sum)
    |> sumCalories
    |> should equal 68802

[<Fact>]
let ``find top 3 elf with the most calories from sample input`` () =
    sampleInput
    |> parse
    |> List.sortByDescending (fun e -> e.Calories |> List.sum)
    |> List.take 3
    |> List.map (fun e -> e.Calories |> List.sum)
    |> List.sum
    |> should equal 45000

[<Fact>]
let ``find top 3 elf with the most calories from test input`` () =
    File.ReadAllText("./Inputs/Day1.txt")
    |> parse
    |> List.sortByDescending (fun e -> e.Calories |> List.sum)
    |> List.take 3
    |> List.map (fun e -> e.Calories |> List.sum)
    |> List.sum
    |> should equal 205370
