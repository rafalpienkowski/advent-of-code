module AdventOfCode.Day7

open System

type HandType =
    | FiveOfKind = 1
    | FourOfKind = 2
    | FullHouse = 3
    | ThreeOfKind = 4
    | TwoPair = 5
    | OnePair = 6
    | HighCard = 7

type Hand = { Cards: int list; Bid: int }
type HandWithType = { Hand: Hand; Type: HandType}

let parseHandWithBid (input: string) : Hand =
    let parseCardInput (input: string) : int =
        match input with
        | "A" -> 14
        | "K" -> 13
        | "Q" -> 12
        //| "J" -> 11 
        | "T" -> 10
        | "9" -> 9
        | "8" -> 8
        | "7" -> 7
        | "6" -> 6
        | "5" -> 5
        | "4" -> 4
        | "3" -> 3
        | "2" -> 2
        | "J" -> 1
        | _ -> failwith $"Unrecognized card: %s{input}"
        
    let parseHand (input: string) : int list =
        [for i in [0..4] do
            yield parseCardInput (string input[i]) ]
    
    let parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    { Cards = parseHand parts[0]; Bid = Int32.Parse parts[1]; }
    
let calculateHandType (hand: int list): HandType =
    
    let groupedAndSortedNumbers (ranks: int list): (int * int) list =
        ranks
        |> List.groupBy id
        |> List.map (fun (key, values) -> key, List.length values)
        |> List.sortByDescending snd
    
    let groups = groupedAndSortedNumbers hand
    let highestGroup = groups[0]
    match highestGroup with
    | _, 5 -> HandType.FiveOfKind
    | _, 4 -> HandType.FourOfKind
    | _, 3 -> if snd groups[1]= 2 then
                  HandType.FullHouse
              else
                  HandType.ThreeOfKind
    | _, 2 -> if snd groups[1] = 2 then
                HandType.TwoPair
              else
                HandType.OnePair
    | _, 1 -> HandType.HighCard
    | _, _ -> failwith "something went wrong"
    
let applyJoker (baseType: HandType) (hand: int list) : HandType =
    let apply (old: HandType) =
        match old with
        | HandType.FiveOfKind -> HandType.FiveOfKind
        | HandType.FourOfKind -> HandType.FiveOfKind
        | HandType.FullHouse -> HandType.FourOfKind
        | HandType.ThreeOfKind -> HandType.FourOfKind
        | HandType.TwoPair -> HandType.FullHouse
        | HandType.OnePair -> HandType.ThreeOfKind
        | HandType.HighCard -> HandType.OnePair
        | _ -> ArgumentOutOfRangeException() |> raise

    let rec applyMultipleTimes oldType remainingJokers =
        match remainingJokers with
        | 0 -> oldType
        | _ -> applyMultipleTimes (apply oldType) (remainingJokers - 1)
    
    let reduceJokers (old: HandType) (jokers: int) : int =
        if old = HandType.OnePair && jokers = 2 then
            1
        else
            jokers
    
    let numberOfJokers = hand |> List.filter (fun x -> x = 1) |> List.length |> reduceJokers baseType
    
    let newType = applyMultipleTimes baseType numberOfJokers
    newType

let addTypeToHand (hand: Hand) : HandWithType =
    { Hand = hand; Type = calculateHandType hand.Cards }
    
let addTypeToHandWithJokers (hand: Hand) : HandWithType =
    let baseType = calculateHandType hand.Cards
    { Hand = hand; Type = applyJoker baseType hand.Cards }
 
let compareHands (hwt1: HandWithType) (hwt2: HandWithType) =
    match hwt1.Type, hwt2.Type with
    | t1, t2 when t1 <> t2 -> compare t1 t2
    | _ ->
        let compareCards card1 card2 =
            compare card2 card1
        List.compareWith compareCards hwt1.Hand.Cards hwt2.Hand.Cards
    
let sortHandType (handsWithType: HandWithType list): HandWithType list =
    handsWithType
    |> List.sortWith compareHands
    |> List.rev

let calculateWinningsForHands (hands: Hand list) : int =
    hands
    |> List.map addTypeToHand
    |> sortHandType
    |> List.map (fun h -> h.Hand.Bid)
    |> List.mapi (fun index value -> (index + 1) * value)
    |> List.sum

let calculateWinningsForHandsWithJoker (hands: Hand list) : int =
    hands
    |> List.map addTypeToHandWithJokers
    |> sortHandType
    |> List.map (fun h -> h.Hand.Bid)
    |> List.mapi (fun index value -> (index + 1) * value)
    |> List.sum