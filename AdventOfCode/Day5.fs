module AdventOfCode.Day5

open System

type Map =
    { DestinationRangeStart: Int64
      SourceRangeStart: Int64
      RangeLength: Int64 }

type Farm =
    { Seeds: Int64 list; Maps: Map list list }

let parseSeeds (seedsStr: string) : Int64 list =
    seedsStr.Substring(seedsStr.IndexOf(":") + 1).Split(" ", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map Int64.Parse
    |> Array.toList

let parseMap (mapStr: string) : Map list =
    mapStr.Substring(mapStr.IndexOf(":") + 1).Split("\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun line ->
        let values = line.Split(" ") |> Array.map Int64.Parse
        { DestinationRangeStart = values[0]; SourceRangeStart = values[1]; RangeLength = values[2] }
    )
    |> Array.toList

let parseFarm (input: string) : Farm =
    let sections = input.Split("\n\n")

    match sections with
    | [| seedsStr; seedToSoilMapStr; soilToFertilizerMapStr; fertilizerToWaterMapStr; waterToLightMapStr; lightToTemperatureMapStr; temperatureToHumidityMapStr; humidityToLocationMapStr |] ->
        let seeds = parseSeeds seedsStr
        let maps = [
            parseMap seedToSoilMapStr;
            parseMap soilToFertilizerMapStr;
            parseMap fertilizerToWaterMapStr;
            parseMap waterToLightMapStr;
            parseMap lightToTemperatureMapStr;
            parseMap temperatureToHumidityMapStr;
            parseMap humidityToLocationMapStr
        ]
        { Seeds = seeds; Maps = maps }
    | _ -> failwith "Invalid input format"
    

let mapSourceToDestination (source: Int64) (maps: Map list): Int64 =
    
    let mapDestinationFor source map  =
        let diff = map.DestinationRangeStart - map.SourceRangeStart
        source + diff

    let map =
        maps
        |> List.tryFind (fun m -> m.SourceRangeStart <= source && (m.SourceRangeStart + m.RangeLength) > source)

    match map with
    | Some map -> mapDestinationFor source map
    | None _ -> source
    
let routeSeedToLocation (seed: Int64) (maps: Map list list): Int64 =
    let rec routeSeed seed mapsList =
        match mapsList with
        | [] -> seed
        | maps :: rest ->
            let transformedSeed = mapSourceToDestination seed maps
            routeSeed transformedSeed rest

    routeSeed seed maps

let routeSeedsToLocation (farm: Farm): Int64 list =
    [for seed in farm.Seeds do
         yield routeSeedToLocation seed farm.Maps]