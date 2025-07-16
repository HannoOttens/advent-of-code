open System
open System.IO
open Shared.Shared

// --------------------------- Part A ------------------------------------------

let paperArea (l,w,h) =
    let lw = l*w
    let wh = w*h
    let hl = h*l
    2 * (lw + wh + hl) + min lw (min wh hl)

let partA inp =
    inp
    |> Seq.map paperArea
    |> Seq.sum

// --------------------------- Part B ------------------------------------------

let ribbonLength (l,w,h) =
    let shortest = min l (min w h)
    let median = max (min l w) (min h (max l w))
    l*w*h + 2*(median+shortest)

let partB inp =
    inp
    |> Seq.map ribbonLength
    |> Seq.sum

// --------------------------- Parsing------------------------------------------

let parseLine (str : string) =
    match str.Split [|'x'|] |> Seq.map int |> Seq.toList with
        | [l;w;h] -> l,w,h
        | _ -> failwith "Invalid input"
let parseInput = Seq.map parseLine

// --------------------------- Entry -------------------------------------------

[<EntryPoint>]
let main argv =
    let input = File.ReadLines "input.txt"
    let parsed = parseInput input
    let total =
        if isPartA argv then
            partA parsed
        else
            partB parsed
    printf "%d" total
    0