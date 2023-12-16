module ``day 16 tests``

open System.IO
open AdventOfCode.Day16
open Xunit
open FsUnit.Xunit

let sampleInput =
    @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|...."

let expectedEnergizedContraption =
    @"######....
.#...#....
.#...#####
.#...##...
.#...##...
.#...##...
.#..####..
########..
.#######..
.#...#.#.."

[<Fact>]
let ``Should energize contraption from sample input`` () =
    let contraption = sampleInput |> loadContraption
    let energized = contraption |> (energize { Position = { X = 0; Y = 0 }; Direction = Right }) |> normalize 
    dump contraption energized |> should equal expectedEnergizedContraption

[<Fact>]
let ``Should count energized contraption from sample input`` () =
    let positions = sampleInput
                    |> loadContraption
                    |> energize { Position = { X = 0; Y = 0 }; Direction = Right }
                    |> normalize
    positions.Length |> should equal 46


[<Fact>]
let ``Should count max energized contraption from sample input`` () =
    sampleInput
    |> loadContraption
    |> getMaxEnergizedContraption
    |> should equal 51
  
[<Fact>]
let ``Should count max energized contraption from test input`` () =
    File.ReadAllText("./Inputs/Day16.txt")
    |> loadContraption
    |> getMaxEnergizedContraption
    |> should equal 7493
    
[<Fact>]
let ``Should count energized contraption from test input`` () =
    let positions = File.ReadAllText("./Inputs/Day16.txt")
                    |> loadContraption
                    |> energize { Position = { X = 0; Y = 0 }; Direction = Right }
                    |> normalize
    positions.Length |> should equal 7060

[<Fact>]
let ``Should move in correct direction for '.'`` () =
    let symbol = '.'

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Up }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 4 }
            Direction = Up } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Down }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 6 }
            Direction = Down } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Right }
        symbol
    |> should
        equal
        [ { Position = { X = 4; Y = 5 }
            Direction = Right } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Left }
        symbol
    |> should
        equal
        [ { Position = { X = 2; Y = 5 }
            Direction = Left } ]


[<Fact>]
let ``Should move in correct direction for '|'`` () =
    let symbol = '|'

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Up }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 4 }
            Direction = Up } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Down }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 6 }
            Direction = Down } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Right }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 4 }
            Direction = Up }
          { Position = { X = 3; Y = 6 }
            Direction = Down } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Left }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 4 }
            Direction = Up }
          { Position = { X = 3; Y = 6 }
            Direction = Down } ]

[<Fact>]
let ``Should move in correct direction for '-'`` () =
    let symbol = '-'

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Right }
        symbol
    |> should
        equal
        [ { Position = { X = 4; Y = 5 }
            Direction = Right } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Left }
        symbol
    |> should
        equal
        [ { Position = { X = 2; Y = 5 }
            Direction = Left } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Up }
        symbol
    |> should
        equal
        [ { Position = { X = 4; Y = 5 }
            Direction = Right }
          { Position = { X = 2; Y = 5 }
            Direction = Left } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Down }
        symbol
    |> should
        equal
        [ { Position = { X = 4; Y = 5 }
            Direction = Right }
          { Position = { X = 2; Y = 5 }
            Direction = Left } ]
        
[<Fact>]
let ``Should move in correct direction for '/'`` () =
    let symbol = '/'

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Right }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 4 }
            Direction = Up } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Up }
        symbol
    |> should
        equal
        [ { Position = { X = 4; Y = 5 }
            Direction = Right } ]

    nextMovesFrom
        { Position = { X = 3; Y = 4 }
          Direction = Left }
        symbol
    |> should
        equal
        [ 
          { Position = { X = 3; Y = 5 }
            Direction = Down } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Down }
        symbol
    |> should
        equal
        [ 
          { Position = { X = 2; Y = 5 }
            Direction = Left } ]

[<Fact>]
let ``Should move in correct direction for '\'`` () =
    let symbol = '\\'

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Right }
        symbol
    |> should
        equal
        [ { Position = { X = 3; Y = 6 }
            Direction = Down } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Up }
        symbol
    |> should
        equal
        [ { Position = { X = 2; Y = 5 }
            Direction = Left } ]

    nextMovesFrom
        { Position = { X = 3; Y = 4 }
          Direction = Left }
        symbol
    |> should
        equal
        [ 
          { Position = { X = 3; Y = 3 }
            Direction = Up } ]

    nextMovesFrom
        { Position = { X = 3; Y = 5 }
          Direction = Down }
        symbol
    |> should
        equal
        [ 
          { Position = { X = 4; Y = 5 }
            Direction = Right } ]
