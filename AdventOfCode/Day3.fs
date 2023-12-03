module AdventOfCode.Day3

open System
open System.Text.RegularExpressions
open Microsoft.FSharp.Core

//https://adventofcode.com/2023/day/3

type Position = { X: int; Y: int }

type Number = { Position: Position; Value: int }

type NumberWithAdjacent = { Position: Position; Value: int; Adjacent: Position list }

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
    
let parseGearsLine (lineNumber: int) (input: string) : Position list =
    let pattern = @"\*"
    let matches = Regex.Matches(input, pattern)
    [ for ``match`` in matches do
          yield { X = ``match``.Index; Y = lineNumber } ]

let parseGears (input: string) : Position list =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
        |> List.ofArray
        |> List.mapi parseGearsLine
        |> List.concat

let parseLineOfSchema (lineNumber: int) (input: string): Schema =
    { PartNumbers = parseNumbers input lineNumber
      Symbols = parseSymbols input lineNumber }

let parseLineOfGearsSchema (lineNumber: int) (input: string): Schema =
    { PartNumbers = parseNumbers input lineNumber
      Symbols = parseGearsLine lineNumber input }

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
    
let parseGearsSchema (input: string) : Schema =
    let startingSchema = { PartNumbers = []; Symbols = [] }

    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    |> List.ofArray
    |> List.mapi parseLineOfGearsSchema
    |> List.fold
        (fun acc s ->
            { PartNumbers = (List.append acc.PartNumbers s.PartNumbers)
              Symbols = (List.append acc.Symbols s.Symbols) })
        startingSchema

let getPartsValueAdjacentToGear (gear: Position) (parts: NumberWithAdjacent list): int =
    let numbers = parts
                    |> List.filter (fun p -> List.exists (fun pa -> pa = gear) p.Adjacent)
                    |> List.map (fun p -> { Value = p.Value; Position = p.Position })
    match numbers.Length with
    | 2 -> numbers[0].Value * numbers[1].Value
    | _ -> 0

let findGearAdjacents (input: Schema) : int list =
    let numbersWithAdjacents = input.PartNumbers
                               |> List.map (fun n -> { Value = n.Value; Position = n.Position; Adjacent = produceAdjacentPositions n })
                               
    input.Symbols
    |> List.map (fun gear -> getPartsValueAdjacentToGear gear numbersWithAdjacents)