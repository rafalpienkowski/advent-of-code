module AdventOfCode2022.Day4

open System

type Range = { Up: int; Down: int }
type Section = { Left: Range; Right: Range }

let toSections (input: string) : Section list =

    let takeSection (section: string) : Section =
        let parts = section.Split([| ','; '-' |]) |> Array.map int

        { Left = { Down = parts[0]; Up = parts[1] }
          Right = { Down = parts[2]; Up = parts[3] } }

    input.Split([| "\n" |], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line -> line |> takeSection)
    |> Array.toList

let findFullyContained (sections: Section list) : Section list =

    let isFullyContained (section: Section) : bool =
        (section.Left.Down <= section.Right.Down && section.Left.Up >= section.Right.Up)
        || (section.Right.Down <= section.Left.Down && section.Right.Up >= section.Left.Up)

    sections |> List.filter isFullyContained
