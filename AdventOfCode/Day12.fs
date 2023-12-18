module AdventOfCode.Day12

type ConditionRecord =
    { Springs: char list; Blocks: int list }

let parseRecord (line: string) : ConditionRecord =
    let parts = line.Split(' ')

    { Springs = parts[0] |> System.String.Concat |> Seq.toList
      Blocks = parts[1].Split(',') |> Seq.map int |> Seq.toList }


let findArrangements (record: ConditionRecord) : int =
    
    0
