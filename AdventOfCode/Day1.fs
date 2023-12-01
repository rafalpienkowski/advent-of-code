module AdventOfCode.Day1

open System
open System.Text
open System.Text.RegularExpressions

//https://adventofcode.com/2023/day/1

let sumFirstAndLastNumberFromList (numbers: string): int =
    let first = numbers[0]
    let last = numbers[numbers.Length - 1]
    Int32.Parse($"{first}{last}")

let extractNumbers (input: string): string =
    let regex = Regex(@"\d+")
    let matches = regex.Matches(input)
    let numbers = matches |> Seq.cast<Match> |> Seq.map (fun matchObj -> matchObj.Value)
    numbers |> String.concat ""

let replaceSpelledOutNumbers (input: string) =
    let numberMap =
            [ "one", "1"
              "two", "2"
              "three", "3"
              "four", "4"
              "five", "5"
              "six", "6"
              "seven", "7"
              "eight", "8"
              "nine", "9" ]
    
    let replaceStartWithNumber (input: string) (numberMap: (string * string) list): string option =
        match
            numberMap
            |> List.tryFind (fun (word, _) -> input.StartsWith(word))
        with
        | Some (_, replacement) -> Some replacement
        | None -> None

    let sb = StringBuilder()
    for i = 0 to String.length input - 1 do
        match replaceStartWithNumber input[i..] numberMap
            with
            | Some replacement -> sb.Append(replacement)
            | None -> sb.Append(input[i])
        |> ignore

    sb.ToString()

let takeNumbers (input: string) : string =
    input
    |> replaceSpelledOutNumbers
    |> extractNumbers

let takeNumberFromLine (line: string) : string =
    Regex(@"\d+").Matches(line)
    |> Seq.cast<Match>
    |> Seq.map (fun matchObj -> matchObj.Value)
    |> String.concat ""
    
let calculateCalibrationValue (input: string) : int =
    input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
        |> List.ofArray
        |> List.map takeNumbers
        |> List.map sumFirstAndLastNumberFromList
        |> List.sum