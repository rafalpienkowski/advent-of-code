module AdventOfCode.Day13

open System
open System.Text

type Reflection = { LeftColumns: int; RowsAbove: int }

let findLineReflection (margin: int) (lines: string array) : int =
    let sb = StringBuilder()

    for line in lines do
        sb.AppendLine(line) |> ignore

    let countDifferences (lines: string array) (idx1: int) (idx2: int) : int =
        let rec countDiffs index acc =
            if index < String.length lines[idx1] then
                let currentDiff = if lines[idx1][index] <> lines[idx2][index] then 1 else 0
                countDiffs (index + 1) (acc + currentDiff)
            else
                acc

        if (idx1 < 0) then 0
        elif (idx2 >= lines.Length) then 0
        else countDiffs 0 0

    let areMirrors (lines: string array) (line: int) : bool =
        let mutable offset = 0
        let mutable areBroken = false

        while offset <= lines.Length / 2 do
            if countDifferences lines (line - offset - 1) (line + offset) > margin then
                areBroken <- true
                offset <- lines.Length

            offset <- offset + 1

        not areBroken

    let mutable mirrorLine = 0
    let mutable currentLine = lines.Length - 1

    while currentLine >= 1 do
        if areMirrors lines currentLine then
            mirrorLine <- currentLine
            currentLine <- 1

        currentLine <- currentLine - 1

    mirrorLine

let findMirror (margin: int) (input: string) : Reflection =

    let convert (lines: string array) : string array =
        Array.init lines[0].Length (fun i -> lines |> Array.map (fun line -> line[i]) |> String.Concat)

    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let rowsAbove = lines |> findLineReflection margin
    let leftColumns = lines |> convert |> findLineReflection margin

    { RowsAbove = rowsAbove
      LeftColumns = leftColumns }

let findPerfectReflection (margin: int) (input: string) : Reflection =
    input.Split("\n\n", StringSplitOptions.None)
    |> Array.map (fun map -> map |> findMirror margin)
    |> Array.fold
        (fun reflection1 reflection2 ->
            { LeftColumns = reflection1.LeftColumns + reflection2.LeftColumns
              RowsAbove = reflection1.RowsAbove + reflection2.RowsAbove })
        { LeftColumns = 0; RowsAbove = 0 }

let summarize (reflection: Reflection) : int =
    reflection.RowsAbove * 100 + reflection.LeftColumns
