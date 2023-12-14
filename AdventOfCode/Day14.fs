module AdventOfCode.Day14

open System
open System.Text

let toControlPanel (input: string) : string list =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    
let dump (controlPanel: string list) : string =
    let dumpBuilder = StringBuilder()
    for line in [0..controlPanel.Length-1] do
        dumpBuilder.AppendLine(controlPanel[line]) |> ignore
    
    dumpBuilder.ToString().TrimEnd()
    
let tilt (controlPanel: string list) : string list =
    let mutable newControlPanel = controlPanel
    
    let canMoveTo (line: int) (position: int): int =
        let mutable checkLine = line
        while checkLine > 0 && newControlPanel[checkLine-1][position] = '.' do
            checkLine <- checkLine - 1
        
        checkLine
    
    let replaceCharInLine (lines: string list) (lineIndex: int) (charIndex: int) (newChar: char) : string list =
        let originalLine = lines[lineIndex]
        let newLine = originalLine.Substring(0, charIndex) + string newChar + originalLine.Substring(charIndex + 1)
        lines
        |> List.mapi (fun i line -> if i = lineIndex then newLine else line)
    
    for line in [1..controlPanel.Length-1] do
        for position in [0..controlPanel[line].Length-1] do
            if newControlPanel[line][position] = 'O'  then
                let possibleLine = canMoveTo line position
                if possibleLine < line then
                    newControlPanel <- replaceCharInLine newControlPanel line position '.'
                    newControlPanel <- replaceCharInLine newControlPanel possibleLine position 'O'
        
    newControlPanel

let calculateLoad (controlPanel: string list) : int =
    let calculateWeightedSum (line: string) (weight: int) =
        line
        |> Seq.sumBy (fun c -> if c = 'O' then weight else 0)
    
    controlPanel
    |> List.mapi (fun i line -> calculateWeightedSum line (List.length controlPanel - i))
    |> List.sum