module AdventOfCode.Day11

open System

type Galactic = { X: Int64; Y: Int64; Id: Int64 }

type Expansions =
    { X: int list
      Y: int list
      Size: Int64 }

let countExpansions (expansion: int) (galactic: string) : Expansions =

    let countExpansionX (galactic: string[]) : int list =
        [ for idx in 0 .. galactic.Length - 1 do
              if galactic[idx].ToCharArray() |> Array.forall (fun c -> c = '.') then
                  yield idx ]

    let countExpansionY (galactic: string[]) : int list =
        [ for idx in 0 .. galactic[0].Length - 1 do
              let testedColumn = galactic |> Array.map (fun line -> line[idx])

              if testedColumn |> Array.forall (fun c -> c = '.') then
                  yield idx ]

    let galacticLines =
        galactic.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)

    { X = countExpansionX galacticLines
      Y = countExpansionY galacticLines
      Size = int64 expansion - 1L }

let findEveryGalactic (galactic: string) : Galactic list =
    let lines = galactic.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let rows = lines.Length - 1
    let columns = lines[0].Length - 1
    let mutable id = 0

    [ for row in [ 0..rows ] do
          for column in [ 0..columns ] do
              if lines[row][column] = '#' then
                  id <- id + 1
                  { Id = id; X = row; Y = column } ]

let findPairs (galactics: Galactic list) : (Galactic * Galactic) list =
    [ for galactic1 in galactics do
          for galactic2 in galactics do
              if galactic1.Id < galactic2.Id then
                  yield (galactic1, galactic2) ]

let findShortestPath2 (galactic1: Galactic) (galactic2: Galactic) (expansions: Expansions) : Int64 =

    let expandsX =
        expansions.X
        |> List.filter (fun e -> (galactic1.X < e && galactic2.X > e) || (galactic2.X < e && galactic1.X > e))
        |> List.length

    let expandsY =
        expansions.Y
        |> List.filter (fun e -> (galactic1.Y < e && galactic2.Y > e) || (galactic2.Y < e && galactic1.Y > e))
        |> List.length

    Math.Abs(galactic2.X - galactic1.X)
    + (int64 expandsX * expansions.Size)
    + Math.Abs(galactic2.Y - galactic1.Y)
    + (int64 expandsY * expansions.Size)
