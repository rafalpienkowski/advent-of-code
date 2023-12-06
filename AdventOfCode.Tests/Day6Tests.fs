module ``day 6 tests``

open System
open AdventOfCode.Day6
open Xunit
open FsUnit.Xunit

let sampleInput = @"Time:      7  15   30
Distance:  9  40  200"

[<Fact>]
let ``Should parse input to race`` () =
    parseInputToLegs sampleInput
    |> should equal [ { Time = 7L; Record = 9L }
                      { Time = 15L; Record = 40L }
                      { Time = 30L; Record = 200L } ]
    
[<Theory>]
[<InlineData( 7L, 9L, 4L)>]
[<InlineData( 15L, 40L, 8L)>]
[<InlineData( 30L, 200L, 9L)>]
let ``Should count leg options`` (time: Int64) (record: Int64) (expectedCount: Int64) =
    countLegOptions { Time = time; Record = record }
    |> should equal expectedCount
    
[<Fact>]
let ``Should calculate options for race from sample input`` () =
    parseInputToLegs sampleInput
    |> List.map countLegOptions
    |> List.fold (fun acc x -> acc * x) 1L
    |> should equal 288L
    
let dayInput = @"Time:        55     99     97     93
Distance:   401   1485   2274   1405"

[<Fact>]
let ``Should calculate options for race from day input`` () =
    parseInputToLegs dayInput
    |> List.map countLegOptions
    |> List.fold (fun acc x -> acc * x) 1L
    |> should equal 2374848L
    
[<Fact>]
let ``Should calculate options for new race from input`` () =
    countLegOptions { Time = 71530L; Record = 940200L }
    |> should equal 71503L
    
    countLegOptions { Time = 55999793L; Record = 401148522741405L }
    |> should equal 39132886L
