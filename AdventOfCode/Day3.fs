module AdventOfCode.Day3

open System
open System.Text.RegularExpressions
open Microsoft.FSharp.Core

//https://adventofcode.com/2023/day/3

type Position = { X: int; Y: int }

type Number = { Position: Position; Value: int }

type Schema =
    { PartNumbers: Number list
      Symbols: Position list }

let parseLine (input: string) : Schema = { PartNumbers = []; Symbols = [] }

let parseNumbers (input: string) (lineNumber: int) : Number list =
    let pattern = @"(?<!\d)\d+"
    let matches = Regex.Matches(input, pattern)

    [ for ``match`` in matches do
          yield
              { Position = { X = ``match``.Index; Y = lineNumber }
                Value = Int32.Parse(``match``.Value) } ]

let parseSymbols (input: string) (lineNumber: int) : Position list =
    let pattern = @"[^\d.]"
    let matches = Regex.Matches(input, pattern)

    [ for ``match`` in matches do
          yield { X = ``match``.Index; Y = lineNumber } ]

let parseLineOfSchema (lineNumber: int) (input: string): Schema =
    { PartNumbers = parseNumbers input lineNumber
      Symbols = parseSymbols input lineNumber }

let parseSchema (input: string) : Schema =
    let startingSchema = { PartNumbers = []; Symbols = [] }

    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> List.ofArray
    |> List.mapi parseLineOfSchema
    |> List.fold
        (fun acc s ->
            { PartNumbers = (List.append acc.PartNumbers s.PartNumbers)
              Symbols = (List.append acc.Symbols s.Symbols) })
        startingSchema

let numberLength (number: Number) : int =
    number.Value.ToString().Length

let produceAdjacentPositions (number: Number) : Position list =
    let numberLength = numberLength number
    [ for y in [number.Position.Y-1..number.Position.Y+1] do
          for x in [number.Position.X-1..number.Position.X+numberLength] do
          yield { X = x; Y = y } ]

let hasAdjacentToSymbol (number: Number) (symbols: Position list) : bool =
    produceAdjacentPositions number
    |> List.exists (fun pos -> List.exists (fun symbolPos -> symbolPos = pos) symbols) 

let findPartNumbersAdjacentToSymbol (schema: Schema) : Number list =
    schema.PartNumbers
    |> List.filter (fun number -> hasAdjacentToSymbol number schema.Symbols)