module AdventOfCode.Day8

open System

type Node =
    { Location: string
      Left: string
      Right: string
      EndsWithZ: bool }

type Network =
    { Navigation: string; Nodes: Node list }

let parse (input: string) : Network =
    let lines = input.Split([| "\n"; "\r" |], StringSplitOptions.RemoveEmptyEntries)
    let navigation = lines[0].Trim()

    let nodes =
        lines.[1..]
        |> Array.map (fun line ->
            let parts = line.Split('=')
            let location = parts[0].Trim()
            let path = parts[1].Trim().Trim('(', ')').Split(',')

            { Location = location
              Left = path[0].Trim()
              Right = path[1].Trim()
              EndsWithZ = location.EndsWith('Z') })
        |> Array.toList

    { Navigation = navigation
      Nodes = nodes }

let moveToNewLocation (node: Node) (navigation: string) (step: int) : string =
    let direction = navigation[step % navigation.Length]
    if direction = 'L' then node.Left else node.Right

let countStepsFromTo (startingNode: Node) (finish: string) (network: Network) (nodeMap: Map<string, Node>) : int =
    let mutable steps = 0
    let mutable currentNode = startingNode

    while currentNode.Location.EndsWith(finish) = false do
        let nextLocation = moveToNewLocation currentNode network.Navigation steps
        currentNode <- nodeMap[nextLocation]
        steps <- steps + 1

    steps

let createNodeMap (network: Network) =
    network.Nodes |> List.map (fun node -> node.Location, node) |> Map.ofList

let countStepsToZZZ (network: Network) : int =
    let nodeMap = createNodeMap network
    let startingNode = network.Nodes |> List.find (fun n -> n.Location = "AAA")
    countStepsFromTo startingNode "ZZZ" network nodeMap

let rec gcd (a: Int64) (b: Int64) : Int64 =
    match (a, b) with
    | x, 0L -> x
    | 0L, y -> y
    | a, b -> gcd b (a % b)

let calculateLcm (a: Int64) (b: Int64) : Int64 = a * b / (gcd a b)

let countGhostSteps (network: Network) : Int64 =
    let nodeMap = createNodeMap network
    let startingNodes = network.Nodes |> List.filter (fun n -> n.Location.EndsWith 'A')
    let mutable lcm = 1L

    for node in startingNodes do
        let count = countStepsFromTo node "Z" network nodeMap
        lcm <- calculateLcm lcm count

    lcm
