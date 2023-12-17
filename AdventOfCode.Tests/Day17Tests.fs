module ``day 17 tests``

open System.IO
open AdventOfCode.Day17
open Xunit
open FsUnit.Xunit

let sampleInput = @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533"

let simplifiedInput = @"24134
32154
32552
34465
45466"

[<Fact>]
let ``Should calculate lost for simplified map`` () =
    simplifiedInput
    |> loadCrucibles
    |> pathHeatLost
    |> should equal 6