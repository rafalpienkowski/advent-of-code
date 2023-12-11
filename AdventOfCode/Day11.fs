module AdventOfCode.Day11

open System
open System.Text

type Galactic = { X: int; Y: int; Id: int }

let expandGalactic (input: string) : string =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    
    let mutable newLines = []
    for line in lines do
        newLines <- newLines @ [ line ]
        if line.ToCharArray() |> Array.forall (fun c -> c = '.') then
            newLines <- newLines @ [ line ]

    let columns = newLines[0].Length
    let mutable newColumns = []
    for column in [0..columns-1] do
        let testedColumn = newLines |> List.map (fun line -> line[column])
        newColumns <- newColumns @ [ testedColumn ]
        if testedColumn |> List.forall (fun c -> c = '.') then
            newColumns <- newColumns @ [ testedColumn ]
    
    let newColumnsLength = newColumns.Length - 1
    let newRowsLength = newColumns[0].Length - 1
    let galacticBuilder = StringBuilder()
    
    for j in [0..newRowsLength] do
        for i in [0..newColumnsLength] do
            galacticBuilder.Append(newColumns[i][j]) |> ignore
        galacticBuilder.AppendLine() |> ignore
    
    galacticBuilder.ToString().TrimEnd()
    
    
let findEveryGalactic (galactic: string) : Galactic list =
    let lines = galactic.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let rows = lines.Length - 1
    let columns = lines[0].Length - 1
    let mutable id = 0
    let galactic = [ for row in [0..rows] do
                        for column in [0..columns] do
                            if lines[row][column] = '#' then
                                id <- id + 1
                                { Id = id; X = row; Y = column }]
    galactic

let findPairs (galactics: Galactic list): (Galactic * Galactic) list =
    [ for galactic1 in galactics do
        for galactic2 in galactics do
            if galactic1.Id < galactic2.Id then
                yield (galactic1, galactic2) ]
    
let findShortestPath (galactic1: Galactic) (galactic2: Galactic) : int =
    Math.Abs(galactic2.X - galactic1.X) + Math.Abs(galactic2.Y - galactic1.Y)