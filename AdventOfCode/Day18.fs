module AdventOfCode.Day18

open System
open System.Text

type Direction =
    | Up
    | Down
    | Left
    | Right

type Position = { X: int; Y: int }

type Move =
    { Length: int
      Direction: Direction
      Color: string }

let rows = Array2D.length1
let columns = Array2D.length2

let parseDigPlan (input: string) : Move list =
    let parseDirection (input: string) : Direction =
        match input with
        | "R" -> Right
        | "U" -> Up
        | "D" -> Down
        | "L" -> Left
        | _ -> failwith "Unrecognized direction"

    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map (fun line ->
        let parts = line.Split(' ')

        { Direction = (parts[0] |> parseDirection)
          Length = (int parts[1])
          Color = parts[2].Trim('(').Trim(')') })

let moveFrom (position: Position) (move: Move) : Position list =
    match move.Direction with
    | Up ->
        [ for idx in [ 1 .. move.Length ] do
              yield { X = position.X; Y = position.Y - idx } ]
    | Down ->
        [ for idx in [ 1 .. move.Length ] do
              yield { X = position.X; Y = position.Y + idx } ]
    | Left ->
        [ for idx in [ 1 .. move.Length ] do
              yield { X = position.X - idx; Y = position.Y } ]
    | Right ->
        [ for idx in [ 1 .. move.Length ] do
              yield { X = position.X + idx; Y = position.Y } ]

let digByPlan (moves: Move list) : Position list =

    let getCurrentPosition (positions: Position list) : Position =
        if positions.Length = 0 then
            { X = 0; Y = 0 }
        else
            positions |> List.last

    let rec dig (currentMoves: Move list) (positions: Position list) =
        if currentMoves.Length = 0 then
            positions
        else
            let currentMove = currentMoves[0]
            let currentPosition = getCurrentPosition positions
            let movePositions = moveFrom currentPosition currentMove
            let newPositions = positions @ movePositions
            let newMoves = currentMoves |> List.tail

            dig newMoves newPositions

    dig moves List.empty


let findMinMax (positions: Position list) : Position * Position =
    let minX = positions |> List.minBy (fun p -> p.X)
    let maxX = positions |> List.maxBy (fun p -> p.X)
    let minY = positions |> List.minBy (fun p -> p.Y)
    let maxY = positions |> List.maxBy (fun p -> p.Y)

    ({ X = minX.X; Y = minY.Y }, { X = maxX.X; Y = maxY.Y })


let closeUp (positions: Position list) : Position list =
    let min, max = positions |> findMinMax
    let minYs = positions |> List.filter( fun p -> p.Y = min.Y)
    [for x in [ min.X .. max.X ] do
         
         yield { X = x; Y = min.Y } ] @ positions

let fillHoles (positions: Position list) : Position list =
    let min, max = positions |> findMinMax
    let mutable newPositions = positions

    let isInside (point: Position) : bool =
        // Check bounding box
        if point.X < min.X || point.X > max.X || point.Y < min.Y || point.Y > max.Y then
            false
        else
            // Ray-casting algorithm
            let rec rayCrossingCount (p: Position) (q: Position) =
                (q.Y > point.Y) <> (p.Y > point.Y)
                && point.X < (p.X - q.X) * (point.Y - q.Y) / (p.Y - q.Y) + q.X

            let mutable crossingCount = 0

            for i = 0 to positions.Length - 1 do
                let j = if i = 0 then positions.Length - 1 else i - 1

                if rayCrossingCount positions[i] positions[j] then
                    crossingCount <- crossingCount + 1

            crossingCount % 2 = 1

    for row in [ 0 .. max.Y ] do
        for column in [ 0 .. max.X ] do
            let position = { X = column; Y = row }

            if not (positions |> List.contains position) && isInside position then
                newPositions <- newPositions @ [ position ]

    newPositions

let countMeters(positions: Position list): int =
    let minY = (positions |> List.minBy (fun p -> p.Y)).Y
    positions
    |> List.filter (fun p -> p.Y > minY)
    |> List.length

let positionsToDigResult (positions: Position list) : char[,] =
    let min, max = positions |> findMinMax
    let rows = max.Y - min.Y + 1
    let columns = max.X - min.X + 1

    Array2D.init rows columns (fun row col ->
        if positions |> List.contains { X = col + min.X; Y = row + min.Y } then
            '#'
        else
            '.')

let dump (digResult: char[,]) : string =
    let digBuilder = StringBuilder()

    for row in [ 0 .. (digResult |> rows) - 1 ] do
        for column in [ 0 .. (digResult |> columns) - 1 ] do
            digBuilder.Append(digResult[row, column]) |> ignore

        digBuilder.AppendLine() |> ignore

    digBuilder.ToString().TrimEnd()
