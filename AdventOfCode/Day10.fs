module AdventOfCode.Day10

open System

type Direction =
    | North
    | South
    | West
    | East

type Position = { X: int; Y: int; }

let convertTo2D (jaggedArray : char array array) : char [,] =
    let rows = Array.length jaggedArray
    let cols = if rows > 0 then Array.length jaggedArray[0] else 0
    Array2D.init<char> rows cols (fun row col -> jaggedArray[col][row])

let parseInputToMaze (input: string): char[,] =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun row -> row.ToCharArray())
    |> convertTo2D
    
let move (current: Position) (direction: Direction) : Position =
    match direction with
    | Direction.North ->{ current with Y = current.Y - 1 }
    | Direction.South -> { current with Y = current.Y + 1 }
    | Direction.East -> { current with X = current.X + 1 }
    | Direction.West -> { current with X = current.X - 1 }

let nextDirection (current: Direction) (pipe: Option<char>) : Option<Direction> =
    match pipe with
    | Some '|' ->
        if current = Direction.South || current = Direction.North then
            Some current
        else
            None
    | Some '-' ->
        if current = Direction.East || current = Direction.West then
            Some current
        else
            None
    | Some 'L' ->
        if current = Direction.West then
            Some Direction.North
        elif current = Direction.South then
            Some Direction.East
        else
            None
    | Some 'J' ->
        if current = Direction.South then
            Some Direction.West
        elif current = Direction.East then
            Some Direction.North
        else
            None
    | Some '7' ->
        if current = Direction.North then
            Some Direction.West
        elif current = Direction.East then
            Some Direction.South
        else
            None
    | Some 'F' ->
        if current = Direction.North then
            Some Direction.East
        elif current = Direction.West then
            Some Direction.South
        else
            None
    | Some '.' -> None
    | Some 'S' -> None
    | None -> None
    | _ -> failwith "Unrecognized pipe"
    
let findStartPosition (matrix: char[,]) : (int * int) option =
    let rows = Array2D.length1 matrix
    let cols = Array2D.length2 matrix

    let rec findPosition row col =
        if row < rows then
            if col < cols then
                if matrix.[row, col] = 'S' then
                    Some (row, col)
                else
                    findPosition row (col + 1)
            else
                findPosition (row + 1) 0
        else
            None

    findPosition 0 0

let getPipeFromMaze (position: Position) (maze: char[,]) : Option<char> =
    if position.X < 0 || position.Y < 0 then
        None
    else
        Some maze[position.X, position.Y]
    
let countLongestLength (input: string): int =
    let maze = input |> parseInputToMaze
    let startingPoint = maze |> findStartPosition
    let mutable longestPath = 0
    let directions = [ Direction.East; Direction.North; Direction.South; Direction.West ]
    
    let countLength (position: Position) (direction: Direction): int =
        let mutable length = 0
        let mutable currentPosition = position
        let mutable currentDirection = direction
        let mutable finished = false
    
        while not finished do
            let nextPosition = move currentPosition currentDirection
            let nextPipe = getPipeFromMaze nextPosition maze
            
            let next = nextDirection currentDirection nextPipe
            match next with
            | Some nextDir ->
                length <- length + 1
                currentPosition <- nextPosition
                currentDirection <- nextDir
            | None ->
                if nextPipe.IsSome && nextPipe.Value = 'S' then
                    length <- (length + 1) / 2
                finished <- true
            
        length
    
    for direction in directions do
        let currentPosition = { X = fst startingPoint.Value; Y = snd startingPoint.Value }
        let length = countLength currentPosition direction
        if length > longestPath then
            longestPath <- length
    
    longestPath