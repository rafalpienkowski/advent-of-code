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

    [ for rucksack in rucksacks do
          yield findItem rucksack 0 ]

let toInt (ch: char) : int =
    let i = int ch
    if i < 91 then i - 38 else i - 96

let sumItems (items: char list) : int =
    let rec sum (chars: char list) (current: int) =
        if chars.Length = 0 then
            current
        else
            sum (chars |> List.tail) (current + (chars[0] |> toInt))

    sum items 0

let getGroupBadge (input: string) : int =
    let groups = input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
                 |> Array.toList
                 |> List.chunkBySize 3
    
    let findCommonItemInGroup (group: string list): char =
        let sortedGroups = group |> List.sortByDescending (_.Length)
        
        let rec findCommonItem (sorted: string list) (index: int) =
            if index > sorted[0].Length then
                failwith "Index out of range"
            elif sorted[1].Contains(sorted[0][index]) && sorted[2].Contains(sorted[0][index]) then
                sorted[0][index]
            else
                findCommonItem sorted (index + 1)
        
        findCommonItem sortedGroups 0
    
    let items = [ for group in groups do
                      yield group |> findCommonItemInGroup ]
    
    sumItems items