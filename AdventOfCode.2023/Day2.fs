module AdventOfCode.Day2

open System
open System.Text.RegularExpressions

//https://adventofcode.com/2023/day/2

type Subset =
    { RedCubes: int
      BlueCubes: int
      GreenCubes: int
    }

type Game =
    { Id: int
      Subsets: Subset list
    }

let getColorCount (input: string) (color: string) : int =
    let pattern = $@"(\d+)\s+{color}"
    let matches = Regex.Match(input, pattern)
    if matches.Success then
        let number = matches.Groups.[1].Value
        (int number)
    else
        0

let parseSubset (input: string) : Subset =
    let redCubes = getColorCount input "red"
    let blueCubes = getColorCount input "blue"
    let greenCubes = getColorCount input "green"
    { RedCubes = redCubes
      BlueCubes = blueCubes
      GreenCubes = greenCubes }

let rec parseSubsets (input: string) : Subset list =
    input.Split(";")
    |> Array.map parseSubset
    |> Array.toList
    
let parseLine (input: string) : Game =
    let fragments = input.Split(":")
    let gameFragment = fragments[0]
    let subsetFragment = fragments[1]
    let firstMatch = Regex(@"\d+").Matches(gameFragment)[0]
    { Id = Int32.Parse(firstMatch.Value)
      Subsets = parseSubsets subsetFragment }
    
let parseGames (input: string) : Game list =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
        |> List.ofArray
        |> List.map parseLine

let cubes =
    { RedCubes = 12
      GreenCubes = 13
      BlueCubes = 14 }

let isSubsetLess (subset: Subset) (cubes: Subset) : bool =
    subset.RedCubes <= cubes.RedCubes &&
    subset.GreenCubes <= cubes.GreenCubes &&
    subset.BlueCubes <= cubes.BlueCubes        
        
let isGamePossible (game: Game) : bool =
    game.Subsets |> List.forall (fun subset -> isSubsetLess subset cubes)
    
let sumGameIds (games: Game list) : int =
    games
    |> List.filter isGamePossible
    |> List.map (fun game -> game.Id)
    |> List.sum
    
let getSubsets (game: Game) : Subset list =
    game.Subsets

let getMaxOf (subset1: Subset) (subset2: Subset) : Subset =
    { RedCubes = if subset1.RedCubes > subset2.RedCubes then subset1.RedCubes else subset2.RedCubes
      GreenCubes = if subset1.GreenCubes > subset2.GreenCubes then subset1.GreenCubes else subset2.GreenCubes
      BlueCubes = if subset1.BlueCubes > subset2.BlueCubes then subset1.BlueCubes else subset2.BlueCubes }

let findMinSetOfCubes (subsets: Subset list) : Subset =
    let minimalCubes = {
        RedCubes = 0
        GreenCubes = 0
        BlueCubes = 0 }
    subsets
    |> List.fold getMaxOf minimalCubes
    
let calculatePowerOf (subset: Subset) : int =
    subset.RedCubes * subset.BlueCubes * subset.GreenCubes