module AdventOfCode.Day9

open System

type PredictedValues = { Values: Int64 list; Prediction: Int64 }

let parseInput (input: string) : Int64 list =
   input.Split([| " " |], StringSplitOptions.RemoveEmptyEntries)
           |> Array.map Int64.Parse
           |> Array.toList 

let hasNonZeroElement (predictedValues: Int64 list) : bool =
    List.exists (fun x -> x <> 0L) predictedValues
    

let calculateNextValues (values: Int64 list): Int64 list =
    [ for i in [0..(values.Length-2)] do
        yield (values[i+1] - values[i]) ]

let predictNextValue (input: string) : Int64 =
    let values = input |> parseInput
    
    let predictNextValues (predictedValues: PredictedValues) : PredictedValues =
        let nextValues = predictedValues.Values |> calculateNextValues
        { Values = nextValues; Prediction = predictedValues.Prediction + List.last nextValues }
    
    let mutable predictedValues = { Values = values; Prediction = values |> List.last }
    
    while predictedValues.Values |> hasNonZeroElement do
        predictedValues <- predictedValues |> predictNextValues
    
    predictedValues.Prediction

let predictPreviousValue (input: string): Int64 =
    let values = input |> parseInput
    let mutable nextValues = values
    let mutable history = [ values ]
    
    let predictHistory (history: Int64 list list): Int64 =
        let mutable predictedHistoryValue = 0L
        for sublist in history |> List.rev do
            predictedHistoryValue <- sublist[0] - predictedHistoryValue
        
        predictedHistoryValue
    
    while nextValues |> hasNonZeroElement do
        nextValues <- nextValues |> calculateNextValues
        history <- history @ [nextValues]
    
    history |> predictHistory