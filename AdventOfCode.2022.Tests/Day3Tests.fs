module ``day 3 should``

open Xunit
open System.IO
open FsUnit.Xunit
open AdventOfCode2022.Day3

let sampleInput = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw"

[<Fact>]
let ``calculate priority for sample input`` () =
    sampleInput
    |> toRucksack
    |> findItems
    |> sumItems
    |> should equal 157
    
[<Fact>]
let ``calculate priority for test input`` () =
    File.ReadAllText("./Inputs/Day3.txt")
    |> toRucksack
    |> findItems
    |> sumItems
    |> should equal 8240

[<Fact>]
let ``get badge for elf group for sample input`` () =
    sampleInput
    |> getGroupBadge
    |> should equal 70
    
    
[<Fact>]
let ``get badge for elf group for test input`` () =
    File.ReadAllText("./Inputs/Day3.txt")
    |> getGroupBadge
    |> should equal 2587
