module AdventOfCode2022.Day3

open System

type Rucksack = { Left: string; Right: string }

let toRucksack (input: string) : Rucksack list =
    input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line ->
        { Left = line.Substring(0, line.Length / 2)
          Right = line.Substring(line.Length / 2) })
    |> Array.toList

let findItems (rucksacks: Rucksack list) : char list =
    
    let rec findItem (rucksack: Rucksack) (index: int) : char =
       if index > rucksack.Left.Length then
           failwith "Index out of range"
       elif rucksack.Right.Contains(rucksack.Left[index]) then
           rucksack.Left[index]
       else
           findItem rucksack (index + 1)
    
    [for rucksack in rucksacks do
         yield findItem rucksack 0]


let sumItems (items: char list) : int =
    
    let toInt (ch: char) : int =
        let i = int ch
        if i < 91 then
            i - 38
        else
            i - 96
    
    let rec sum (chars: char list) (current: int) =
        if chars.Length = 0 then
            current
        else
            sum (chars |> List.tail) (current + (chars[0] |> toInt))
    
    sum items 0
