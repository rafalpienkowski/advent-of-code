module AdventOfCode.Day4

open System

type ScratchCard =
    { Id: string
      WinningNumbers: int list
      MyNumbers: int list }

let parseCardsFrom (input: string) : int list =
    input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map int
    |> List.ofArray

let parseScratchGameFrom (input: string) : ScratchCard =
    let parts = input.Split(":", StringSplitOptions.RemoveEmptyEntries)
    let gameId = parts[0]
    let cards = parts[1].Split("|", StringSplitOptions.RemoveEmptyEntries)
    let winningNumbers = cards[0]
    let myNumbers = cards[1]
    { Id = gameId
      WinningNumbers = parseCardsFrom winningNumbers
      MyNumbers = parseCardsFrom myNumbers }

let parseScratchGamesFrom (input: string) : ScratchCard list =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map parseScratchGameFrom

let getWinningNumbersCount (scratchCard: ScratchCard) : int =
    scratchCard.MyNumbers
    |> List.filter (fun number -> List.contains number scratchCard.WinningNumbers)
    |> List.length
    
let calculatePointsForGame (number: int) : int =
    pown 2 (number - 1)