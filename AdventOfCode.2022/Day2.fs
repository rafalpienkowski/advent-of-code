module AdventOfCode2022.Day2

open System

type Move =
    | Rock
    | Paper
    | Scissors

type Leg = { Opponent: Move; Me: Move }

type Strategy = { Legs: Leg list }

let loadStrategyGuide (input: string) : Strategy =
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

    { Legs =
        input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s ->
            { Opponent = s |> parseOpponentMove
              Me = s |> parseMyMove })
        |> Array.toList }

let calculateSum (strategy: Strategy) : int =

    let moveScore (move: Move) : int =
        match move with
        | Rock -> 1
        | Paper -> 2
        | Scissors -> 3

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
    
    sumLegs strategy.Legs 0
