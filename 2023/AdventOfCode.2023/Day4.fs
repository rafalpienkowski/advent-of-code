module AdventOfCode.Day4

open System

type ScratchCard =
    { Id: int
      WinningNumbers: int list
      MyNumbers: int list }
    
type ScratchCardCopies =
    { Copies: int
      ScratchCardId: int }

type ScratchCardHand = { Cards: ScratchCardCopies list }

let parseCardsFrom (input: string) : int list =
    input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map int
    |> List.ofArray

let parseScratchGameFrom (input: string) : ScratchCard =
    let parts = input.Split(":", StringSplitOptions.RemoveEmptyEntries)
    let gameId = Int32.Parse(parts[0].Substring(4))
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

let calculatePointsForGame (number: int) : int = pown 2 (number - 1)

let gainCopies (scratchCard: ScratchCard) (hand: ScratchCardHand) : ScratchCardHand =
    let winningNumbersCount = getWinningNumbersCount scratchCard
    let currentMultiplier = hand.Cards
                            |> List.find (fun c -> c.ScratchCardId = scratchCard.Id)
    
    let incrementCopies (card: ScratchCardCopies) =
        if card.ScratchCardId > scratchCard.Id && card.ScratchCardId <= scratchCard.Id + winningNumbersCount then
            { card with Copies = card.Copies + 1 * currentMultiplier.Copies }
        else
            card

    let updatedCards =
        hand.Cards
        |> List.map incrementCopies

    { hand with Cards = updatedCards }
    
