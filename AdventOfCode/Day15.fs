module AdventOfCode.Day15

open System
open Microsoft.FSharp.Collections

type Lens = { Label: string; FocalLength: int }

type Operation =
    | Remove
    | Save // Insert or Update

type InitialisationStep = { Lens: Lens; Action: Operation }

type Box = { Id: int; Lens: Lens seq }

let hash (input: string) : int =

    let rec calculate (current: string) (sum: int) : int =
        if current.Length = 0 then
            sum
        else
            let charValue = int current[0]
            let newSum = ((sum + charValue) * 17) % 256

            calculate (current.Substring(1, current.Length - 1)) newSum

    calculate input 0

let loadInitialisation (input: string) : InitialisationStep array =

    let toStep (step: string) : InitialisationStep =
        let removeIdx = step.IndexOf('-')
        let saveIdx = step.IndexOf('=')
        let signIdx = [| removeIdx; saveIdx |] |> Array.max

        { Action = if removeIdx > 0 then Operation.Remove else Operation.Save
          Lens =
            { Label = step.Substring(0, signIdx)
              FocalLength =
                if removeIdx > 0 then
                    0
                else
                    int (step.Substring(signIdx + 1)) } }

    input.Split(',', StringSplitOptions.RemoveEmptyEntries) |> Array.map toStep


let initialise (input: string) : Box array =
    let steps = input |> loadInitialisation
    let boxes = Array.init 256 (fun i -> { Id = i; Lens = Seq.empty })

    let rec initialiseStep (boxes: Box array) (steps: InitialisationStep array) =
        if steps.Length = 0 then
            boxes
        else
            let newBoxes =
                boxes
                |> Array.mapi (fun i x ->
                    if i = (steps[0].Lens.Label |> hash) then
                        match steps[0].Action with
                        | Remove ->
                            { x with
                                Lens = x.Lens |> Seq.where (fun l -> l.Label <> steps[0].Lens.Label) }
                        | Save ->
                            if x.Lens |> Seq.exists (fun l -> l.Label = steps[0].Lens.Label) then
                                { x with
                                    Lens =
                                        x.Lens
                                        |> Seq.map (fun l ->
                                            if l.Label = steps[0].Lens.Label then steps[0].Lens else l) }
                            else
                                { x with
                                    Lens = x.Lens |> Seq.append (steps[0].Lens |> Seq.singleton) }
                    else
                        x)

            initialiseStep newBoxes (steps |> Array.tail)

    initialiseStep boxes steps

let calculateFocusingPowerFor (boxes: Box array) : int =

    let rec calculateForBox (boxes: Box array) (currentValue: int) (idx: int) =

        if boxes.Length = 0 then
            currentValue
        else
            let newValue =
                boxes[0].Lens
                |> Seq.rev
                |> Seq.fold
                    (fun (acc, index) lens -> (acc + (idx * index * lens.FocalLength), index + 1))
                    (currentValue, 1)
                |> fst

            calculateForBox (boxes |> Array.tail) newValue (idx + 1)

    calculateForBox boxes 0 1
