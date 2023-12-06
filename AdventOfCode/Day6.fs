module AdventOfCode.Day6

open System

type Leg = { Time: int; Record: int }

let parseInputToLegs (input: string) : Leg list =
    let takeNumbersFromLine (line: string): int list =
        line.Substring(line.IndexOf(":") + 1).Split(" ", StringSplitOptions.RemoveEmptyEntries)
        |> Array.map int
        |> Array.toList
    
    let produceLegsFromNumbers (times: int list) (records: int list): Leg list =
        [for i in [0..times.Length-1] do
             yield { Time = times[i]; Record = records[i] } ]
    
    let numbers = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map takeNumbersFromLine
    
    produceLegsFromNumbers numbers[0] numbers[1]
 
let calculateDistance (seconds: int) (remainingTime: int): int =
    seconds * remainingTime
    
let countLegOptions (leg: Leg) : int =
    [for i in [1..leg.Time-1] do
        let distance = calculateDistance i (leg.Time - i)
        if distance > leg.Record then
            yield 1
        else
            yield 0
        ]
    |> List.sum