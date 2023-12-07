module ``day 7 tests``

open System
open System.IO
open AdventOfCode.Day7
open Microsoft.FSharp.Collections
open Xunit
open FsUnit.Xunit

let sampleInput = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483"

[<Fact>]
let ``Should parse sample hand with bid`` () =
    parseHandWithBid "32T3K 765"
    |> should equal { Bid = 765
                      Cards = [ 3; 2; 10; 3; 13 ] }
    
    parseHandWithBid "T55J5 684"
    |> should equal { Bid = 684
                      Cards = [ 10; 5; 5; 1; 5 ] }
    
    parseHandWithBid "KK677 28"
    |> should equal { Bid = 28
                      Cards = [ 13; 13; 6; 7; 7 ] }
    
    parseHandWithBid "KTJJT 220"
    |> should equal { Bid = 220
                      Cards = [ 13; 10; 1; 1; 10 ] }
    
    parseHandWithBid "QQQJA 483"
    |> should equal { Bid = 483
                      Cards = [ 12; 12; 12; 1; 14 ] }

let sampleCards : obj[] list =
    [
        [| [ 3; 2; 10; 3; 13 ]; HandType.OnePair |]
        [| [ 10; 5; 5; 11; 5 ]; HandType.ThreeOfKind |]
        [| [ 13; 13; 6; 7; 7 ]; HandType.TwoPair |]
        [| [ 13; 10; 11; 11; 10 ]; HandType.TwoPair |]
        [| [ 12; 12; 12; 11; 14 ]; HandType.ThreeOfKind |]
        [| [ 12; 12; 12; 11; 11 ]; HandType.FullHouse |]
    ]
    
[<Theory>]
[<MemberData(nameof(sampleCards))>]
let ``Should calculate hand type`` (cards: int list) (expectedType: HandType) =
    calculateHandType cards
    |> should equal expectedType
    
[<Fact>]
let ``Should calculate for sample`` () =
    sampleInput.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map parseHandWithBid
    |> calculateWinningsForHands
    |> should equal 6440

[<Fact>]
let ``Should calculate for sample with joker`` () =
    sampleInput.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map parseHandWithBid
    |> calculateWinningsForHandsWithJoker
    |> should equal 5905
    
[<Fact>]
let ``Should calculate for input with jokers`` () =
    File.ReadAllText("./Inputs/Day7/input.txt").Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map parseHandWithBid
    |> calculateWinningsForHandsWithJoker
    |> should equal 250665248

let betterInput = @"2345A 1
Q2KJJ 13
Q2Q2Q 19
T3T3J 17
T3Q33 11
2345J 3
J345A 2
32T3K 5
T55J5 29
KK677 7
KTJJT 34
QQQJA 31
JJJJJ 37
JAAAA 43
AAAAJ 59
AAAAA 61
2AAAA 23
2JJJJ 53
JJJJ2 41"

[<Fact>]
let ``Should calculate for sample with joker better input`` () =
    betterInput.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList
    |> List.map parseHandWithBid
    |> calculateWinningsForHandsWithJoker
    |> should equal 6839
