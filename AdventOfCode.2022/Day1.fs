module AdventOfCode2022.Day1

open System

type Elf = { Calories: int list }

let parse (input: string) : Elf list =
    input.Split([| "\n\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun group ->
        { Calories =
            group.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
            |> Array.map int
            |> Array.toList })
    |> Array.toList

let sumCalories (elf: Elf) : int =
    elf.Calories |> List.sum