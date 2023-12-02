module ``day 2 tests``

open System.IO
open AdventOfCode
open Xunit
open Day2
open FsUnit.Xunit

let sampleInput = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"

[<Fact>]
let ``Should parse sample input lines`` () =
    parseGames sampleInput
    |> should haveLength 5
    
[<Fact>]
let ``Should parse sample game ids`` () =
    parseGames sampleInput
    |> List.map (fun game -> game.Id)
    |> should equal [ 1;2;3;4;5 ]

[<Fact>]
let ``Should parse games`` () =
    let singleLine = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
    parseGames singleLine
    |> should equal [
                      { Id = 1
                        Subsets = [
                          {
                              RedCubes = 4
                              BlueCubes = 3
                              GreenCubes = 0 
                          }
                          {
                              RedCubes = 1
                              BlueCubes = 6
                              GreenCubes = 2
                          }
                          {
                              RedCubes = 0
                              BlueCubes = 0
                              GreenCubes = 2
                             }
                         ]
                        }
                      ]
    
[<Theory>]
[<InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)>]
[<InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", true)>]
[<InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", false)>]
[<InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", false)>]
[<InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", true)>]
let ``Should determine if game is possible`` (game: string) (isPossible: bool) =
    parseLine game
    |> isGamePossible
    |> should equal isPossible
    
[<Fact>]
let ``Should sum game ids for possible games`` () =
    parseGames sampleInput
    |> sumGameIds
    |> should equal 8
    
[<Fact>]
let ``Should calculate result for day 2`` () =
    File.ReadAllText("./Inputs/Day2/input.txt")
    |> parseGames
    |> sumGameIds
    |> should equal 2283

[<Theory>]
[<InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 4, 2, 6)>]
[<InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 1, 3, 4)>]
[<InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 20, 13, 6)>]
[<InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 14, 3, 15)>]
[<InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 6, 3, 2)>]
let ``Should calculate the min cube subset for`` (gameLine: string) red green blue =
    let expectedSubset = { RedCubes = red
                           GreenCubes = green
                           BlueCubes = blue }
    parseLine gameLine
    |> getSubsets
    |> findMinSetOfCubes
    |> should equal expectedSubset

[<Theory>]
[<InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 48)>]
[<InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12)>]
[<InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 1560)>]
[<InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 630)>]
[<InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 36)>]
let ``Should calculate the power of subset`` (gameLine: string) power =
    parseLine gameLine
    |> getSubsets
    |> findMinSetOfCubes
    |> calculatePowerOf
    |> should equal power

let secondInput = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"

[<Fact>]
let ``Should calculate sum of the power of min sets`` () =
    parseGames secondInput
    |> List.map getSubsets
    |> List.map findMinSetOfCubes
    |> List.map calculatePowerOf
    |> List.sum
    |> should equal 2286
    
[<Fact>]
let ``Should calculate power of subset for day 2`` () =
    File.ReadAllText("./Inputs/Day2/input.txt")
    |> parseGames
    |> List.map getSubsets
    |> List.map findMinSetOfCubes
    |> List.map calculatePowerOf
    |> List.sum
    |> should equal 78669

