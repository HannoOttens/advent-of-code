open System.IO
open Shared.Shared

// --------------------------- Part A ------------------------------------------

let toNumb (c : char) =
    match c with
        | '(' -> 1
        | ')' -> -1
        | _ -> failwith "Invalid input"

let partA input = input |> Seq.map toNumb |> Seq.sum

// --------------------------- Part B ------------------------------------------

let first (index, value) element =
    if value < 0 then
        index, -1
    else
        index + 1, value + element

let partB input = input |> Seq.map toNumb |> Seq.fold first (0,0) |> fst

// --------------------------- Entry -------------------------------------------<EntryPoint>]

[<EntryPoint>]
let main argv =
    let input = File.ReadAllText "input.txt"
    let total =
        if isPartA argv then
            partA input
        else
            partB input
    printf "%d" total
    0