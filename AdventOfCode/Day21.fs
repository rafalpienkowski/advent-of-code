module AdventOfCode.Day21

open System
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

type Direction =
    | Up
    | Down
    | Left
    | Right

type Field =
    | Start
    | Garden
    | Rock

type Position = { X: int; Y: int }
type Garden = Field[,]

let gardenRows = Array2D.length1
let gardenCols = Array2D.length2

let parseGarden (input: string) : Garden =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let numRows = Array.length lines
    let numCols = lines[0].Length

    let parseField (f: char) : Field =
        match f with
        | 'S' -> Start
        | '.' -> Garden
        | '#' -> Rock
        | _ -> failwith "Invalid field"

    Array2D.init numRows numCols (fun row col -> parseField (char lines[row].[col]))

let findGardenPlots (stepsToWalk: Int64) (garden: Garden) : Position list =

    let rows = gardenRows garden
    let cols = gardenCols garden
    
    let findStartPosition: Position option =

        let rec findPosition row col =
            if row < rows then
                if col < cols then
                    if garden[row, col] = Start then
                        Some({ X = row; Y = col })
                    else
                        findPosition row (col + 1)
                else
                    findPosition (row + 1) 0
            else
                None

        findPosition 0 0

    let walks (position: Position) : Position list =
        
        let walk (direction: Direction) : Position Option =
            if direction = Up && garden[position.X, (position.Y + rows - 1) % rows] <> Rock then
                Some { position with Y = (position.Y + rows - 1) % rows }
            elif direction = Down && garden[position.X, (position.Y + 1) % rows] <> Rock then
                Some { position with Y = (position.Y + 1) % rows }
            elif direction = Left && garden[(position.X + cols - 1) % cols, position.Y] <> Rock then
                Some { position with X = (position.X + cols - 1) % cols }
            elif direction = Right && garden[(position.X + 1) % cols, position.Y ] <> Rock then
                Some { position with X = (position.X + 1) % cols }
            else
                None
        
        let pos = [ walk Up; walk Down; walk Left; walk Right ] |> List.choose id
        pos

    let rec doSteps (steps: Int64) (positions: Position list) : Position list =
        if steps = 0 then
            positions
        else
            let newPositions =
                positions
                |> List.map walks
                |> List.fold (fun acc posList -> acc @ posList) []
                |> List.distinct

            doSteps (steps - 1L) newPositions

    let start = findStartPosition
    doSteps stepsToWalk [ start.Value ]