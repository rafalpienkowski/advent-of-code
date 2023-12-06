module AdventOfCode.Day6

open System

type Leg = { Time: Int64; Record: Int64 }

let parseInputToLegs (input: string) : Leg list =
    let takeNumbersFromLine (line: string): Int64 list =
        line.Substring(line.IndexOf(":") + 1).Split(" ", StringSplitOptions.RemoveEmptyEntries)
        |> Array.map Int64.Parse
        |> Array.toList
    
    let produceLegsFromNumbers (times: Int64 list) (records: Int64 list): Leg list =
        [for i in [0..times.Length-1] do
             yield { Time = times[i]; Record = records[i] } ]
    
    let numbers = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.map takeNumbersFromLine
    
    produceLegsFromNumbers numbers[0] numbers[1]
 
let calculateDistance (seconds: Int64) (remainingTime: Int64): Int64 =
    seconds * remainingTime
    
let countLegOptions (leg: Leg) : Int64 =
    let time = leg.Time - 1L
    let mutable idx = 1L
    let mutable sum = 0L
    
    while idx < time do
        if (calculateDistance idx (leg.Time - idx)) > leg.Record then
            sum <- sum + 1L
        
        idx <- idx + 1L
    
    sum