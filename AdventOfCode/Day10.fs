module AdventOfCode.Day10

open System
open Microsoft.FSharp.Core

type Direction =
    | North
    | South
    | West
    | East

type Position = { X: int; Y: int }

let parseMaze (input: string) : char [,] =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let numRows = Array.length lines
    let numCols = lines |> Array.map (fun line -> line.Length) |> Array.max

    Array2D.init numRows numCols (fun row col -> lines[row][col])

let move (current: Position) (direction: Direction) : Position =
    match direction with
    | Direction.North -> { current with X = current.X - 1 }
    | Direction.South -> { current with X = current.X + 1 }
    | Direction.East -> { current with Y = current.Y + 1 }
    | Direction.West -> { current with Y = current.Y - 1 }

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
        if current = Direction.West then Some Direction.North
        elif current = Direction.South then Some Direction.East
        else None
    | Some 'J' ->
        if current = Direction.South then Some Direction.West
        elif current = Direction.East then Some Direction.North
        else None
    | Some '7' ->
        if current = Direction.North then Some Direction.West
        elif current = Direction.East then Some Direction.South
        else None
    | Some 'F' ->
        if current = Direction.North then Some Direction.East
        elif current = Direction.West then Some Direction.South
        else None
    | Some '.' -> None
    | Some 'S' -> None
    | None -> None
    | _ -> failwith "Unrecognized pipe"

let mazeRows = Array2D.length1
let mazeCols = Array2D.length2
    

let findStartPosition (maze: char[,]) : Position option =
    let rows = mazeRows maze
    let cols = mazeCols maze

    let rec findPosition row col =
        if row < rows then
            if col < cols then
                if maze[row, col] = 'S' then
                    Some({ X = row; Y = col })
                else
                    findPosition row (col + 1)
            else
                findPosition (row + 1) 0
        else
            None

    findPosition 0 0

let getPipeFromMaze (position: Position) (maze: char[,]) : Option<char> =
    if
        position.X < 0
        || position.Y < 0
        || position.X >= mazeRows maze 
        || position.Y >= mazeCols maze
    then
        None
    else
        Some maze[position.X, position.Y]

let countLongestLength (input: string) : int =
    let maze = input |> parseMaze
    let startingPoint = maze |> findStartPosition
    let mutable longestPath = 0

    let directions =
        [ Direction.East; Direction.North; Direction.South; Direction.West ]

    let countLength (position: Position) (direction: Direction) : int =
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
        let currentPosition = startingPoint
        let length = countLength currentPosition.Value direction

        if length > longestPath then
            longestPath <- length

    longestPath

let isNotPipe (c: char) : bool = c = '.'

let isInMaze (position: Position) (maze: char[,]) : bool =
    position.X >= 0
    && position.X < mazeRows maze
    && position.Y >= 0
    && position.Y < mazeCols maze

let getLoop (maze: char [,]) : Position list =
    let startingPosition = maze |> findStartPosition
    let directions =
        [ Direction.East; Direction.North; Direction.South; Direction.West ]
        
    let findLoop (position: Position) (direction: Direction) : Option<Position list> =
        let mutable currentPosition = position
        let mutable currentDirection = direction
        let mutable finished = false
        let mutable isLoop = false
        let mutable loop = [ position ]

        while not finished do
            
            let nextPosition = move currentPosition currentDirection
            let nextPipe = getPipeFromMaze nextPosition maze
            let next = nextDirection currentDirection nextPipe

            match next with
            | Some nextDir ->
                let currentPipe = getPipeFromMaze currentPosition maze
                if currentPipe.IsSome && currentPipe.Value <> '.' then
                    loop <- loop @ [ currentPosition ]
                currentPosition <- nextPosition
                currentDirection <- nextDir
            | None ->
                if nextPipe.IsSome && nextPipe.Value = 'S' then
                    isLoop <- true
                    loop <- loop @ [ currentPosition ]

                finished <- true
        
        if isLoop then
            Some loop
        else
            None
   
    let mutable idx = 0
    let mutable loop = None
    while idx < 4 do
        loop <- findLoop startingPosition.Value directions[idx]
        match loop with
        | None -> idx <- idx + 1
        | Some _ -> idx <- 4
    
    loop.Value
 
let checkNestDirections (maze: char[,]) (position: Position) (loop: Position list): bool =
    let directions =
        [ Direction.East; Direction.North; Direction.South; Direction.West ]
    let mutable isNest = true

    let crossLoop (loop: Position list) (position: Position) (symbolOpt: Option<char>) (direction: Direction) (isNest: bool): bool =
        if symbolOpt.IsNone || not (loop |> List.contains position) then
            isNest
        else
            let symbol = symbolOpt.Value
            match direction with
            | North ->
                symbol = '-' || symbol = 'F' || symbol = '7' || symbol = 'S'
            | South ->
                symbol = '-'  || symbol = 'L' || symbol = 'J' || symbol = 'S'
            | West ->
                symbol = '|' || symbol = 'F' || symbol = 'L' || symbol = 'S'
            | East ->
                symbol = '|' || symbol = 'J' || symbol = '7' || symbol = 'S'

    let checkNestDirection (maze: char[,]) (position: Position) (direction: Direction) : bool =
        let mutable currentPosition = position
        let mutable isNest = false

        while isInMaze currentPosition maze do
            let newPosition = move currentPosition direction
            let symbol = getPipeFromMaze newPosition maze
            isNest <- crossLoop loop newPosition symbol direction isNest
            currentPosition <- newPosition

        isNest
    
    for direction in directions do
        if isNest then
            isNest <- checkNestDirection maze position direction

    isNest

let printLoop (maze: char[,]) (loop: Position list): unit =
    let rows = mazeRows maze
    let cols = mazeCols maze
    
    for row in [ 0 .. rows - 1 ] do
        for col in [ 0 .. cols - 1 ] do
            let position = { X = row; Y = col }
            if loop |> List.contains position then
                Console.Write "X"
            else
                Console.Write "-"
        Console.WriteLine ""

let calculateNestCount (input: string) : int =
    let maze = input |> parseMaze
    let rows = mazeRows maze
    let cols = mazeCols maze
    let loop = (getLoop maze) |> List.distinct
    printLoop maze loop
    let mutable nest = 0
    
    for row in [ 0 .. rows - 1 ] do
        for col in [ 0 .. cols - 1 ] do
            let position = { X = row; Y = col }
            if not (loop |> List.contains position) then
                let symbol  = getPipeFromMaze position maze
                if symbol.IsSome && isNotPipe symbol.Value then
                    if checkNestDirections maze position loop then
                        nest <- nest + 1

    nest

