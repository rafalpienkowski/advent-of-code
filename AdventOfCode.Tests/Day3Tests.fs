module ``day 3 tests``

open System.IO
open AdventOfCode
open AdventOfCode.Day3
open Xunit
open Day3
open FsUnit.Xunit

[<Fact>]
let ``Should parse numbers with positions from  line`` ()  =
    parseNumbers "467..114.." 0
    |> should equal  [
                        {   Position = { X = 0; Y = 0 }
                            Value = 467
                        }
                        {   Position = { X = 5; Y = 0 }
                            Value = 114
                        }
                    ]
    
    parseNumbers "...*......" 1
    |> should be Empty

    parseNumbers "..35..633." 2
    |> should equal [
                        {   Position = { X = 2; Y = 2 }
                            Value = 35
                        }
                        {   Position = { X = 6; Y = 2 }
                            Value = 633
                        }
                    ]
    
    parseNumbers "......#..." 3
    |> should be Empty
    
    parseNumbers "617*......" 4
    |> should equal [
                        {   Position = { X = 0; Y = 4 }
                            Value = 617
                        }
                    ]
    
    parseNumbers ".....+.58." 5
    |> should equal [
                        {   Position = { X = 7; Y = 5 }
                            Value = 58
                        }
                    ]
    
    parseNumbers "..592....." 6
    |> should equal [
                        {   Position = { X = 2; Y = 6 }
                            Value = 592
                        }
                    ]
    
    parseNumbers "......755." 7
    |> should equal [
                        {   Position = { X = 6; Y = 7 }
                            Value = 755
                        }
                    ]

    parseNumbers "...$.*...." 8
    |> should be Empty
    
    
    parseNumbers ".664.598.." 9
    |> should equal [
                        {   Position = { X = 1; Y = 9 }
                            Value = 664
                        }
                        {   Position = { X = 5; Y = 9 }
                            Value = 598
                        }
                    ]

[<Fact>]
let ``Should parse symbols`` () =
    parseSymbols "467..114.." 0
    |> should be Empty
    
    parseSymbols "...*......" 1
    |> should equal [ { X = 3; Y = 1 } ]

    parseSymbols "..35..633." 2
    |> should be Empty
    
    parseSymbols "......#..." 3
    |> should equal [ { X = 6; Y = 3 } ]
    
    parseSymbols "617*......" 4
    |> should equal [ { X = 3; Y = 4 } ]
    
    parseSymbols ".....+.58." 5
    |> should equal [ { X = 5; Y = 5 } ]
    
    parseSymbols "..592....." 6
    |> should be Empty
    
    parseSymbols "......755." 7
    |> should be Empty

    parseSymbols "...$.*...." 8
    |> should equal [ { X = 3; Y = 8 }
                      { X = 5; Y = 8 } ]
    
    parseSymbols ".664.598.." 9
    |> should be Empty

let schemaInput = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598.."

[<Fact>]
let ``Should parse schema input`` () =
    parseSchema schemaInput
    |> should equal {
                        PartNumbers = [
                             {   Position = { X = 0; Y = 0 }
                                 Value = 467 }
                             {   Position = { X = 5; Y = 0 }
                                 Value = 114 }
                             {   Position = { X = 2; Y = 2 }
                                 Value = 35 }
                             {   Position = { X = 6; Y = 2 }
                                 Value = 633 }
                             {   Position = { X = 0; Y = 4 }
                                 Value = 617 }
                             {   Position = { X = 7; Y = 5 }
                                 Value = 58 }
                             {   Position = { X = 2; Y = 6 }
                                 Value = 592 }
                             {   Position = { X = 6; Y = 7 }
                                 Value = 755 }
                             {   Position = { X = 1; Y = 9 }
                                 Value = 664 }
                             {   Position = { X = 5; Y = 9 }
                                 Value = 598 }
                        ]
                        Symbols =  [
                            { X = 3; Y = 1 }
                            { X = 6; Y = 3 }
                            { X = 3; Y = 4 }
                            { X = 5; Y = 5 }
                            { X = 3; Y = 8 }
                            { X = 5; Y = 8 }
                        ]
                    }
    
[<Fact>]
let ``Should find part numbers adjacent to a symbol`` () =
    parseSchema schemaInput
    |> findPartNumbersAdjacentToSymbol
    |> should equal [
                         {   Position = { X = 0; Y = 0 }
                             Value = 467 }
                         {   Position = { X = 2; Y = 2 }
                             Value = 35 }
                         {   Position = { X = 6; Y = 2 }
                             Value = 633 }
                         {   Position = { X = 0; Y = 4 }
                             Value = 617 }
                         {   Position = { X = 2; Y = 6 }
                             Value = 592 }
                         {   Position = { X = 6; Y = 7 }
                             Value = 755 }
                         {   Position = { X = 1; Y = 9 }
                             Value = 664 }
                         {   Position = { X = 5; Y = 9 }
                             Value = 598 }
                    ]

[<Fact>]
let ``Should sum part numbers adjacent to a symbol`` () =
    parseSchema schemaInput
    |> findPartNumbersAdjacentToSymbol
    |> List.map (fun n -> n.Value)
    |> List.sum
    |> should equal 4361
    
[<Fact>]
let ``Should calculate result for day 3`` () =
    File.ReadAllText("./Inputs/Day3/input.txt")
    |> parseSchema
    |> findPartNumbersAdjacentToSymbol
    |> List.map (fun n -> n.Value)
    |> List.sum
    |> should equal 4361
