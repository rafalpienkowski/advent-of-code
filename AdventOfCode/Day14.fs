module AdventOfCode.Day14

open System
open System.Text

let toControlPanel (input: string) : StringBuilder list =
    input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> StringBuilder(line))
    |> Array.toList

let dump (controlPanel: StringBuilder list) : string =
    let dumpBuilder = StringBuilder()

    for line in [ 0 .. controlPanel.Length - 1 ] do
        dumpBuilder.AppendLine(controlPanel[line].ToString()) |> ignore

    dumpBuilder.ToString().TrimEnd()

let tilt (controlPanel: StringBuilder list) : StringBuilder list =

    let canMoveTo (line: int) (position: int) : int =
        let mutable checkLine = line

        while checkLine > 0 && controlPanel[checkLine - 1][position] = '.' do
            checkLine <- checkLine - 1

        checkLine

    for line in [ 1 .. controlPanel.Length - 1 ] do
        for position in [ 0 .. controlPanel[line].Length - 1 ] do
            if controlPanel[line][position] = 'O' then
                let possibleLine = canMoveTo line position

                if possibleLine < line then
                    controlPanel[line][position] <- '.'
                    controlPanel[possibleLine][position] <- 'O'

    controlPanel

let calculateLoad (controlPanel: StringBuilder list) : int =
    let calculateWeightedSum (line: StringBuilder) (weight: int) =
        line.ToString() |> Seq.sumBy (fun c -> if c = 'O' then weight else 0)

    controlPanel
    |> List.mapi (fun i line -> calculateWeightedSum line (List.length controlPanel - i))
    |> List.sum

let rotate90DegreesClockwise (controlPanel: StringBuilder list) : StringBuilder list =
    let numRows = controlPanel.Length
    let numCols = if numRows > 0 then controlPanel.[0].Length else 0

    let getCharAt (line: int) (position: int) : char =
        controlPanel.[numCols - position - 1].[line]

    let rotateLine (lineIndex: int) : StringBuilder =
        let newLine = StringBuilder(numRows)

        for position in 0 .. numCols - 1 do
            newLine.Append(getCharAt lineIndex position) |> ignore

        newLine

    [ for lineIndex in 0 .. numRows - 1 -> rotateLine lineIndex ]

let cycle (numberOfCycles: int) (controlPanel: StringBuilder list) : StringBuilder list =

    let hash (controlPanel: StringBuilder list) : string =
        let hashBuilder = StringBuilder()

        for line in [ 0 .. controlPanel.Length - 1 ] do
            hashBuilder.Append($"{controlPanel[line].ToString()},") |> ignore

        hashBuilder.ToString().TrimEnd()
    
    
    let fullCycle (panel: StringBuilder list) : StringBuilder list =
        panel
        |> tilt // north
        |> rotate90DegreesClockwise
        |> tilt // west
        |> rotate90DegreesClockwise
        |> tilt // south
        |> rotate90DegreesClockwise
        |> tilt //east
        |> rotate90DegreesClockwise

    let rec applyCycles (seen: Map<string, int>) (remainingCycles: int) (currentResult: StringBuilder list) =
        if remainingCycles = 0 then
            currentResult
        else
            let hash = currentResult |> hash
            if
                (seen |> Map.containsKey hash)
                && ((1000000000 - seen[hash]) % (numberOfCycles - remainingCycles - seen[hash]) = 0)
            then
                applyCycles Map.empty 0 currentResult
            else
                let updatedResult = fullCycle currentResult
                let idx = numberOfCycles - remainingCycles
                applyCycles
                    (seen |> Map.add hash idx)
                    (remainingCycles - 1)
                    updatedResult

    applyCycles Map.empty numberOfCycles controlPanel
