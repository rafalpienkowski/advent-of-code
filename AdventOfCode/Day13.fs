module AdventOfCode.Day13

open System
open System.Text

type Reflection = { LeftColumns: int; RowsAbove: int }

let findLineReflection (lines: string array) : int =
    let sb = StringBuilder()
    for line in lines do
        sb.AppendLine(line) |> ignore
        
    let areLinesSimilar (lines: string array) (idx1: int) (idx2: int) : bool =
        if (idx1 < 0) then true
        elif (idx2 >= lines.Length) then true
        else lines[idx1] = lines[idx2]

    let areMirrors (lines: string array) (line: int) : bool =
        let mutable offset = 0
        let mutable areBroken = false

        while offset <= lines.Length / 2 do
            if not (areLinesSimilar lines (line - offset - 1) (line + offset)) then
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

let findMirror (input: string) : Reflection =

    let convert (lines: string array) : string array =
        Array.init lines[0].Length (fun i -> lines |> Array.map (fun line -> line[i]) |> String.Concat)
        
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let rowsAbove = lines |> findLineReflection
    let leftColumns = lines |> convert |> findLineReflection

    { RowsAbove = rowsAbove
      LeftColumns = leftColumns }

let findPerfectReflection (input: string) : Reflection =
    
    let temp = input.Split("\n\n", StringSplitOptions.None)
                |> Array.map (fun map -> map |> findMirror)
    
    input.Split("\n\n", StringSplitOptions.None)
    |> Array.map (fun map -> map |> findMirror)
    |> Array.fold
        (fun reflection1 reflection2 ->
            { LeftColumns = reflection1.LeftColumns + reflection2.LeftColumns
              RowsAbove = reflection1.RowsAbove + reflection2.RowsAbove })
        { LeftColumns = 0; RowsAbove = 0 }

let summarize (reflection: Reflection) : int =
    reflection.RowsAbove * 100 + reflection.LeftColumns
