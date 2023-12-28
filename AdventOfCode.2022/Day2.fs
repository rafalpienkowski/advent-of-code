module AdventOfCode2022.Day2

open System

type Move =
    | Rock
    | Paper
    | Scissors

type LegResult =
    | Lose
    | Draw
    | Win

type Leg = { Opponent: Move; Me: Move }

type LegGuide = { Opponent: Move; Result: LegResult }

let parseOpponentMove (moves: string) : Move =
    let move = moves.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]

    match move with
    | "A" -> Rock
    | "B" -> Paper
    | "C" -> Scissors
    | _ -> failwith "Unknown opponent's move"

let parseMyMove (moves: string) : Move =
    let move = moves.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]

    match move with
    | "X" -> Rock
    | "Y" -> Paper
    | "Z" -> Scissors
    | _ -> failwith "Unknown my move"

let loadStrategy (input: string) : Leg list =
    input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun s ->
        { Opponent = s |> parseOpponentMove
          Me = s |> parseMyMove })
    |> Array.toList

let parseResult (moves: string) : LegResult =
    let move = moves.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]

    match move with
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> failwith "Unknown my move"

let loadGuides (input: string) : LegGuide list =
    input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun s ->
        { Opponent = s |> parseOpponentMove
          Result = s |> parseResult })
    |> Array.toList

let moveScore (move: Move) : int =
    match move with
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3

let calculateSum (strategy: Leg list) : int =

    let legScore (leg: Leg) : int =
        if leg.Me = leg.Opponent then
            3
        elif
            (leg.Me = Rock && leg.Opponent = Scissors)
            || (leg.Me = Scissors && leg.Opponent = Paper)
            || (leg.Me = Paper && leg.Opponent = Rock)
        then
            6
        else
            0

    let rec sumLegs (legs: Leg list) (current: int) : int =
        if legs.Length = 0 then
            current
        else
            sumLegs (legs |> List.tail) (current + (moveScore legs[0].Me) + (legScore legs[0]))

    sumLegs strategy 0

let calculateStrategy (legGuides: LegGuide list) : int =

    let legScore (result: LegResult) : int =
        match result with
        | Lose -> 0
        | Draw -> 3
        | Win -> 6

    let myMove (leg: LegGuide) : Move =
        match leg.Result with
        | Lose ->
            match leg.Opponent with
            | Rock -> Scissors
            | Paper -> Rock
            | Scissors -> Paper
        | Draw -> leg.Opponent
        | Win ->
            match leg.Opponent with
            | Rock -> Paper
            | Paper -> Scissors
            | Scissors -> Rock

    let rec sumLegGuides (legs: LegGuide list) (current: int) : int =
        if legs.Length = 0 then
            current
        else
            let moveScore = legs[0] |> myMove |> moveScore
            let legScore = legScore legs[0].Result
            sumLegGuides (legs |> List.tail) (current + moveScore + legScore)

    sumLegGuides legGuides 0
