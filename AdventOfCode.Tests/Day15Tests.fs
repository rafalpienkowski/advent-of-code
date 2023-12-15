module ``day 15 tests``

open System
open System.IO
open AdventOfCode.Day15
open Xunit
open FsUnit.Xunit

let hashSample = "HASH"

[<Fact>]
let ``Should calculate hash for hash input`` () = hashSample |> hash |> should equal 52

let sampleInput = @"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"

[<Fact>]
let ``Should calculate hash for sample input`` () =
    sampleInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map hash
    |> Array.sum
    |> should equal 1320

[<Fact>]
let ``Should calculate hash for test input`` () =
    File
        .ReadAllText("./Inputs/Day15/input.txt")
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
    |> Array.map hash
    |> Array.sum
    |> should equal 521341


[<Fact>]
let ``Should load initialisation script from sample input`` () =
    sampleInput
    |> loadInitialisation
    |> should
        equal
        [| { Lens = { Label = "rn"; FocalLength = 1 }
             Action = Save }
           { Lens = { Label = "cm"; FocalLength = 0 }
             Action = Remove }
           { Lens = { Label = "qp"; FocalLength = 3 }
             Action = Save }
           { Lens = { Label = "cm"; FocalLength = 2 }
             Action = Save }
           { Lens = { Label = "qp"; FocalLength = 0 }
             Action = Remove }
           { Lens = { Label = "pc"; FocalLength = 4 }
             Action = Save }
           { Lens = { Label = "ot"; FocalLength = 9 }
             Action = Save }
           { Lens = { Label = "ab"; FocalLength = 5 }
             Action = Save }
           { Lens = { Label = "pc"; FocalLength = 0 }
             Action = Remove }
           { Lens = { Label = "pc"; FocalLength = 6 }
             Action = Save }
           { Lens = { Label = "ot"; FocalLength = 7 }
             Action = Save } |]

[<Fact>]
let ``Should calculate focus power for boxes from sample input`` () =
    sampleInput |> initialise |> calculateFocusingPowerFor |> should equal 145

[<Fact>]
let ``Should calculate focus power for boxes from test input`` () =
    File.ReadAllText("./Inputs/Day15/input.txt")
    |> initialise
    |> calculateFocusingPowerFor
    |> should equal 252782
