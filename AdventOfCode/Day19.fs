module AdventOfCode.Day19

open System
open System.Text.RegularExpressions

type Part = Map<char, int>

type Rule =
    { PartType: char
      Operator: char
      Threshold: int
      Destination: string }

type Workflow = string * Rule list

let parseWorkflow (input: string) : Workflow =
    let rulesStartingPosition = input.IndexOf('{')
    let name = input.Substring(0, rulesStartingPosition)

    let findFirstMatchingChar (str: string) (chars: char list) : char option = Seq.tryFind str.Contains chars

    let parseRule (input: string) =
        let indexOfName = input.IndexOf(':')
        let destination = input.Substring(indexOfName + 1, input.Length - indexOfName - 1)
        let pattern = @"\d+"
        let matchResult = Regex.Match(input, pattern)
        let threshold = int matchResult.Groups[0].Value

        match findFirstMatchingChar input [ '<'; '>' ] with
        | Some operator ->
            { PartType = input[0]
              Operator = operator
              Threshold = threshold
              Destination = destination }
        | _ -> failwith "Invalid input rule"

    let noConditionRule (destination: string) : Rule =
        { PartType = ' '
          Operator = ' '
          Threshold = 0
          Destination = destination }

    let rules =
        input
            .Substring(rulesStartingPosition + 1, input.Length - 2 - rulesStartingPosition)
            .Split(',')
        |> Array.map (fun rule ->
            if rule.Contains('<') || rule.Contains('>') then
                rule |> parseRule
            else
                rule |> noConditionRule)
        |> Array.toList

    (name, rules)

let parsePart (input: string) : Part =
    input.Substring(1, input.Length - 2).Split(',')
    |> Array.map (fun s ->
        let desc = s.Split('=')
        char desc[0], int desc[1])
    |> Map.ofArray

let parseInput (input: string) : Workflow list * Part list =
    let workflowPartSeparator = "\n\n"
    let partsSeparator = "\n"

    let workflowsAndParts =
        input.Split([| workflowPartSeparator; partsSeparator |], StringSplitOptions.RemoveEmptyEntries)

    let workflows =
        workflowsAndParts
        |> Array.filter (fun s -> not (s.Contains('=')))
        |> Array.map parseWorkflow
        |> Array.toList

    let parts =
        workflowsAndParts
        |> Array.filter (fun s -> s.Contains('='))
        |> Array.map parsePart
        |> Array.toList

    workflows, parts

let rec performOperation (part: Part) (rules: Rule list) : string =
    let rule = rules[0]

    if rule.PartType = ' ' then
        rule.Destination
    else
        let partValue = part[rule.PartType]

        if rule.Operator = '>' && partValue > rule.Threshold then
            rule.Destination
        elif rule.Operator = '<' && partValue < rule.Threshold then
            rule.Destination
        else
            performOperation part (rules |> List.tail)

let processParts (workflows: Workflow list) (parts: Part list) : int list =
    let workflowsMap =
        workflows
        |> List.fold (fun acc (name, rules) -> Map.add name rules acc) Map.empty<string, Rule list>

    let rec runWorkflow (part: Part) (workflowName: string) : string =
        let workflow = workflowsMap[workflowName]
        let operationResult = performOperation part workflow

        if operationResult = "R" || operationResult = "A" then
            operationResult
        else
            runWorkflow part operationResult

    let rec processParts (parts: Part list) (results: int list) =
        if parts.Length = 0 then
            results
        else
            let currentPart = parts[0]
            let result = runWorkflow currentPart "in"

            if result = "A" then
                processParts (parts |> List.tail) (results @ [ currentPart.Values |> Seq.sum ])
            else
                processParts (parts |> List.tail) results

    processParts parts List.empty
