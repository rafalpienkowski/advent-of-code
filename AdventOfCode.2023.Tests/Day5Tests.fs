module ``day 5 tests``

open System.IO
open AdventOfCode.Day5
open Xunit
open FsUnit.Xunit

let sampleInput = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4"

[<Fact>]
let ``Should parse input`` () =
    parseFarm sampleInput
    |> should equal { Seeds = [ 79L; 14L; 55L; 13L ]
                      Maps = [
                          [{ DestinationRangeStart = 50L; SourceRangeStart = 98L; RangeLength = 2L }
                           { DestinationRangeStart = 52L; SourceRangeStart = 50L; RangeLength = 48L }]
                          [{ DestinationRangeStart = 0L; SourceRangeStart = 15L; RangeLength = 37L }
                           { DestinationRangeStart = 37L; SourceRangeStart = 52L; RangeLength = 2L }
                           { DestinationRangeStart = 39L; SourceRangeStart = 0L; RangeLength = 15L }]
                          [{ DestinationRangeStart = 49L; SourceRangeStart = 53L; RangeLength = 8L }
                           { DestinationRangeStart = 0L; SourceRangeStart = 11L; RangeLength = 42L }
                           { DestinationRangeStart = 42L; SourceRangeStart = 0L; RangeLength = 7L }
                           { DestinationRangeStart = 57L; SourceRangeStart = 7L; RangeLength = 4L }]
                          [{ DestinationRangeStart = 88L; SourceRangeStart = 18L; RangeLength = 7L }
                           { DestinationRangeStart = 18L; SourceRangeStart = 25L; RangeLength = 70L }]
                          [{ DestinationRangeStart = 45L; SourceRangeStart = 77L; RangeLength = 23L }
                           { DestinationRangeStart = 81L; SourceRangeStart = 45L; RangeLength = 19L }
                           { DestinationRangeStart = 68L; SourceRangeStart = 64L; RangeLength = 13L }]
                          [{ DestinationRangeStart = 0L; SourceRangeStart = 69L; RangeLength = 1L }
                           { DestinationRangeStart = 1L; SourceRangeStart = 0L; RangeLength = 69L }]
                          [{ DestinationRangeStart = 60L; SourceRangeStart = 56L; RangeLength = 37L }
                           { DestinationRangeStart = 56L; SourceRangeStart = 93L; RangeLength = 4L }]
                      ] }

[<Theory>]
[<InlineData(79L,81L)>]
[<InlineData(14L,14L)>]
[<InlineData(55L,57L)>]
[<InlineData(13L,13L)>]
let ``Should create source to destination`` source destination =
    let maps = [ { DestinationRangeStart = 50L; SourceRangeStart = 98L; RangeLength = 2L }
                 { DestinationRangeStart = 52L; SourceRangeStart = 50L; RangeLength = 48L } ]
    
    mapSourceToDestination source maps
    |> should equal destination
 
[<Fact>]
let ``Should find lowest locations for farm seeds`` () =
  sampleInput
  |> parseFarm
  |> routeSeedsToLocation
  |> should equal 35L
  
[<Fact>]
let ``Should find lowest locations for farm seeds from data`` () =
  File.ReadAllText("./Inputs/Day5.txt")
  |> parseFarm
  |> routeSeedsToLocation
  |> should equal 462648396L

[<Fact>]
let ``Should find lowest locations for farm seed ranges`` () =
  sampleInput
  |> parseRangedFarm
  |> routeSeedRangesToLocation
  |> should equal 46L
