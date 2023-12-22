module ``day 19 tests``

open System
open System.IO
open AdventOfCode.Day19
open Xunit
open FsUnit.Xunit

let sampleWorkflow = "ex{x>10:one,m<20:two,a>30:R,A}"

[<Fact>]
let ``Should parse sample workflow`` () =
    sampleWorkflow
    |> parseWorkflow
    |> should
        equal
        ("ex",
         [ { PartType = 'x'
             Operator = '>'
             Threshold = 10
             Destination = "one" }
           { PartType = 'm'
             Operator = '<'
             Threshold = 20
             Destination = "two" }
           { PartType = 'a'
             Operator = '>'
             Threshold = 30
             Destination = "R" }
           { PartType = ' '
             Operator = ' '
             Threshold = 0
             Destination = "A" } ])

let sampleInput =
    @"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}"

[<Fact>]
let ``Should determine which accepted workflows and returns sum of part`` () =
    let workflows, parts = sampleInput |> parseInput
    (processParts workflows parts) |> should equal [ 7540; 4623; 6951 ]

[<Fact>]
let ``Should determine which accepted workflows and returns sum of parts`` () =
    let workflows, parts = sampleInput |> parseInput
    (processParts workflows parts) |> List.sum |> should equal 19114

[<Fact>]
let ``Should calculate sum of parts for test input`` () =
    let workflows, parts = File.ReadAllText("./Inputs/Day19.txt") |> parseInput
    (processParts workflows parts) |> List.sum |> should equal 389114

[<Fact>]
let ``Should take paths for sample input`` () =
    let workflows, _ = sampleInput |> parseInput
    workflows
    |> takeCombinations
    |> calculateCombinations
    |> should equal 167409079868000L
    
    
[<Fact>]
let ``Should take paths for test data`` () =
    let workflows, _ = File.ReadAllText("./Inputs/Day19.txt") |> parseInput
    workflows
    |> takeCombinations
    |> calculateCombinations
    |> should equal 167409079868000L
