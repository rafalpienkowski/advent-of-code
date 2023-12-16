module AdventOfCode.Day16

open System
open System.Text

type Direction =
    | Up
    | Down
    | Left
    | Right

type Position = { X: int; Y: int }

type Move =
    { Position: Position
      Direction: Direction }

let rows = Array2D.length1
let columns = Array2D.length2

let loadContraption (input: string) : char[,] =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let numRows = Array.length lines
    let numCols = lines |> Array.map (fun line -> line.Length) |> Array.max

    Array2D.init numRows numCols (fun row col -> lines[col][row])

let nextPosition (move: Move) : Position =
    match move.Direction with
    | Up ->
        { move.Position with
            Y = move.Position.Y - 1 }
    | Down ->
        { move.Position with
            Y = move.Position.Y + 1 }
    | Left ->
        { move.Position with
            X = move.Position.X - 1 }
    | Right ->
        { move.Position with
            X = move.Position.X + 1 }

let nextMovesFrom (move: Move) (symbol: char) : Move list =
    match symbol with
    | '.' ->
        [ { Direction = move.Direction
            Position = move |> nextPosition } ]
    | '\\' ->
        match move.Direction with
        | Up ->
            [ { Direction = Left
                Position =
                  { X = move.Position.X - 1
                    Y = move.Position.Y } } ]
        | Down ->
            [ { Direction = Right
                Position =
                  { X = move.Position.X + 1
                    Y = move.Position.Y } } ]
        | Left ->
            [ { Direction = Up
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y - 1 } } ]
        | Right ->
            [ { Direction = Down
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y + 1 } } ]
    | '/' ->
        match move.Direction with
        | Up ->
            [ { Direction = Right
                Position =
                  { X = move.Position.X + 1
                    Y = move.Position.Y } } ]
        | Down ->
            [ { Direction = Left
                Position =
                  { X = move.Position.X - 1
                    Y = move.Position.Y } } ]
        | Left ->
            [ { Direction = Down
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y + 1 } } ]
        | Right ->
            [ { Direction = Up
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y - 1 } } ]
    | '-' ->
        if move.Direction = Right || move.Direction = Left then
            [ { Direction = move.Direction
                Position = move |> nextPosition } ]
        else
            [ { Direction = Right
                Position =
                  { X = move.Position.X + 1
                    Y = move.Position.Y } }
              { Direction = Left
                Position =
                  { X = move.Position.X - 1
                    Y = move.Position.Y } } ]
    | '|' ->
        if move.Direction = Up || move.Direction = Down then
            [ { Direction = move.Direction
                Position = move |> nextPosition } ]
        else
            [ { Direction = Up
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y - 1 } }
              { Direction = Down
                Position =
                  { X = move.Position.X
                    Y = move.Position.Y + 1 } } ]
    | _ -> failwith "Unrecognized element"

let energize (initial: Move) (contraption: char[,]) : Map<Move, char> =
    let energized = Map.empty

    let isInContraption (position: Position) : bool =
        position.X >= 0
        && position.Y >= 0
        && position.X < (contraption |> rows)
        && position.Y < (contraption |> columns)

    let tailIfMultipleElements lst =
        match lst with
        | [] -> [] // Empty list
        | _ :: rest -> rest

    let rec light (currentMoves: Move list) (currentEnergized: Map<Move, char>) : Map<Move, char> =
        if currentMoves.Length = 0 then
            currentEnergized
        else
            let currentMove = currentMoves[0]

            if
                not (currentMove.Position |> isInContraption)
                || (currentEnergized |> Map.containsKey currentMove)
            then
                light (currentMoves |> tailIfMultipleElements) currentEnergized
            else
                let newEnergized = currentEnergized |> Map.add currentMove '#'
                let currentSymbol = contraption[currentMove.Position.X, currentMove.Position.Y]

                let nextMoves =
                    (currentMoves |> tailIfMultipleElements)
                    @ (nextMovesFrom currentMove currentSymbol)

                light nextMoves newEnergized

    light [ initial ] energized

let normalize (energized: Map<Move, char>) : Position list =
    energized.Keys
    |> Seq.map (fun move -> move.Position)
    |> Seq.distinct
    |> List.ofSeq

let dump (contraption: char[,]) (energized: Position list) : string =
    let contraptionBuilder = StringBuilder()

    for row in [ 0 .. (contraption |> rows) - 1 ] do
        for column in [ 0 .. (contraption |> columns) - 1 ] do
            if energized |> List.contains ({ X = column; Y = row }) then
                contraptionBuilder.Append('#') |> ignore
            else
                contraptionBuilder.Append('.') |> ignore

        contraptionBuilder.AppendLine() |> ignore

    contraptionBuilder.ToString().TrimEnd()
