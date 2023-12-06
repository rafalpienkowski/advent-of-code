module ``day 6 tests``

open AdventOfCode.Day6
open Xunit
open FsUnit.Xunit

let sampleInput = @"Time:      7  15   30
Distance:  9  40  200"

[<Fact>]
let ``Should parse input to race`` () =
    parseInputToLegs sampleInput
    |> should equal [ { Time = 7; Record = 9 }
                      { Time = 15; Record = 40 }
                      { Time = 30; Record = 200 } ]
    
[<Theory>]
[<InlineData( 7, 9, 4)>]
[<InlineData( 15, 40, 8)>]
[<InlineData( 30, 200, 9)>]
let ``Should count leg options`` (time: int) (record: int) (expectedCount: int) =
    countLegOptions { Time = time; Record = record }
    |> should equal expectedCount
    
[<Fact>]
let ``Should calculate options for race from sample input`` () =
    parseInputToLegs sampleInput
    |> List.map countLegOptions
    |> List.fold (fun acc x -> acc * x) 1
    |> should equal 288
    
let dayInput = @"Time:        55     99     97     93
Distance:   401   1485   2274   1405"

[<Fact>]
let ``Should calculate options for race from day input`` () =
    parseInputToLegs dayInput
    |> List.map countLegOptions
    |> List.fold (fun acc x -> acc * x) 1
    |> should equal 2374848
    
