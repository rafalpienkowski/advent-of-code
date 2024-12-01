module AdventOfCode.Day17

open System
open System.Text

type Position = { X: int; Y: int }
type Analysis = { Lost: int; Visited: bool }

type Crucibles = Map<Position, int>
type CruciblesAnalysis = Map<Position, Analysis>

let rows = Array2D.length1
let columns = Array2D.length2

let loadCrucibles (input: string) : Crucibles =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let numRows = Array.length lines
    let numCols = lines |> Array.map (fun line -> line.Length) |> Array.max
    let crucibles = Map.empty

    for row in [0 .. (numRows - 1)] do
        for col in [0 .. (numCols - 1)] do
            if row = 0 && col = 0 then
                crucibles |> Map.add ({ X = row; Y = col }, 0) |> ignore
            else
                let lost = int lines[col].[row]
                crucibles |> Map.add ({ X = row; Y = col }, lost) |> ignore
                
    crucibles

let getNeighborsFor (position: Position) : Position list =
    []

let pathHeatLost (crucibles: Crucibles) : int =
    let analysis = crucibles
                    |> Map.toSeq
                    |> Seq.map ( fun (key, _) -> key, { Lost = Int32.MaxValue; Visited = false })
                    |> Map.ofSeq
    let initial = { X = 0; Y = 0 }
    
    let rec findPath (positionsToCheck: Position list) (analysis: CruciblesAnalysis) =
        if positionsToCheck.Length = 0 then
            0
        else
            let currentPosition = positionsToCheck[0]
            let newPositionsToCheck = positionsToCheck |> List.tail
            findPath newPositionsToCheck analysis
    
    findPath [ initial ] analysis
    
    
let dump (crucibles: int[,]) (path: Position list) : string =
    let contraptionBuilder = StringBuilder()

    for row in [ 0 .. (crucibles |> rows) - 1 ] do
        for column in [ 0 .. (crucibles |> columns) - 1 ] do
            if path |> List.contains { X = column; Y = row } then
                contraptionBuilder.Append('#') |> ignore
            else
                contraptionBuilder.Append('.') |> ignore

        contraptionBuilder.AppendLine() |> ignore

    contraptionBuilder.ToString().TrimEnd()
