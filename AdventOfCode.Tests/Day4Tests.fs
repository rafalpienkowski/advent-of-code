module ``day 4 tests``

open System.IO
open AdventOfCode.Day4
open Xunit
open FsUnit.Xunit

[<Fact>]
let ``Should parse scratchcards`` () =
    let line1 = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
    parseScratchGameFrom line1
    |> should
        equal
        { Id = "Card 1"
          WinningNumbers = [ 41; 48; 83; 86; 17 ]
          MyNumbers = [ 83; 86; 6; 31; 17; 9; 48; 53 ] }

    let line2 = "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19"
    parseScratchGameFrom line2
    |> should
        equal
        { Id = "Card 2"
          WinningNumbers = [ 13; 32; 20; 16; 61 ]
          MyNumbers = [ 61; 30; 68; 82; 17; 32; 24; 19 ] }
    
    let line3 = "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1"
    parseScratchGameFrom line3
    |> should
        equal
        { Id = "Card 3"
          WinningNumbers = [ 1; 21; 53; 59; 44 ]
          MyNumbers = [ 69; 82; 63; 72; 16; 21; 14; 1 ] }
        
    let line4 = "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83"
    parseScratchGameFrom line4
    |> should
        equal
        { Id = "Card 4"
          WinningNumbers = [ 41; 92; 73; 84; 69 ]
          MyNumbers = [ 59; 84; 76; 51; 58; 5; 54; 83 ] }
        
    let line5 = "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36"
    parseScratchGameFrom line5
    |> should
        equal
        { Id = "Card 5"
          WinningNumbers = [ 87; 83; 26; 28; 32 ]
          MyNumbers = [ 88; 30; 70; 12; 93; 22; 82; 36 ] }
        
    let line6 = "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
    parseScratchGameFrom line6
    |> should
        equal
        { Id = "Card 6"
          WinningNumbers = [ 31; 18; 13; 56; 72 ]
          MyNumbers = [ 74; 77; 10; 23; 35; 67; 36; 11 ] }

[<Fact>]
let ``Should get winning numbers from games`` () =
    let scratchCard1 =
        { Id = "Card 1"
          WinningNumbers = [ 41; 48; 83; 86; 17 ]
          MyNumbers = [ 83; 86; 6; 31; 17; 9; 48; 53 ] }
    getWinningNumbersCount scratchCard1
    |> should equal 4

    let scratchCard2 =
        { Id = "Card 2"
          WinningNumbers = [ 13; 32; 20; 16; 61 ]
          MyNumbers = [ 61; 30; 68; 82; 17; 32; 24; 19 ] }
    getWinningNumbersCount scratchCard2
    |> should equal 2
    
    let scratchCard3 =
        { Id = "Card 3"
          WinningNumbers = [ 1; 21; 53; 59; 44 ]
          MyNumbers = [ 69; 82; 63; 72; 16; 21; 14; 1 ] }
    getWinningNumbersCount scratchCard3
    |> should equal 2
        
    let scratchCard4 =
        { Id = "Card 4"
          WinningNumbers = [ 41; 92; 73; 84; 69 ]
          MyNumbers = [ 59; 84; 76; 51; 58; 5; 54; 83 ] }
    getWinningNumbersCount scratchCard4
    |> should equal 1
        
    let scratchCard5 =
        { Id = "Card 5"
          WinningNumbers = [ 87; 83; 26; 28; 32 ]
          MyNumbers = [ 88; 30; 70; 12; 93; 22; 82; 36 ] }
    getWinningNumbersCount scratchCard5
    |> should equal 0
        
    let scratchCard6 =
        { Id = "Card 6"
          WinningNumbers = [ 31; 18; 13; 56; 72 ]
          MyNumbers = [ 74; 77; 10; 23; 35; 67; 36; 11 ] }
    getWinningNumbersCount scratchCard6
    |> should equal 0

let inputString = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"

[<Fact>]
let ``Calculate sum of points from input string`` () =
    parseScratchGamesFrom inputString
    |> List.map getWinningNumbersCount
    |> List.map calculatePointsForGame
    |> List.sum
    |> should equal 13
    
[<Fact>]
let ``Calculate sum of points for input`` () =
    File.ReadAllText("./Inputs/Day4/input.txt")
    |> parseScratchGamesFrom 
    |> List.map getWinningNumbersCount
    |> List.map calculatePointsForGame
    |> List.sum
    |> should equal 23441
