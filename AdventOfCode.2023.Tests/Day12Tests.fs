module ``day 12 tests``

open System.IO
open AdventOfCode.Day12
open Xunit
open FsUnit.Xunit


[<Theory>]
[<InlineData("???.### 1,1,3", 1)>]
//[<InlineData(".??..??...?##. 1,1,3", 4)>]
//[<InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)>]
//[<InlineData("????.#...#... 4,1,1", 1)>]
//[<InlineData("????.######..#####. 1,6,5", 4)>]
//[<InlineData("?###???????? 3,2,1", 10)>]
let ``Should find arrangement for condition record`` (line: string) (expectedArrangements: int) =
    line
    |> parseRecord
    |> findArrangements
    |> should equal expectedArrangements