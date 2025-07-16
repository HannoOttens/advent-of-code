open System
open System.IO
open Shared.Shared

// --------------------------- Part A ------------------------------------------

let walk (set, (x,y)) char =
    let newP =
        match char with
        | '>' -> x+1, y
        | 'v' -> x  , y+1
        | '<' -> x-1, y
        | '^' -> x  , y-1
        | _ -> failwith "Invalid input"
    Set.add newP set, newP

let locations inp =
    inp
    |> Seq.fold walk (Set.ofList [(0,0)], (0,0))
    |> fst

let partA inp = inp |> locations |> Set.count

// --------------------------- Part B ------------------------------------------

let mod2Index odd inp =
    inp
    |> Seq.mapi (fun i el -> i,el)
    |> Seq.filter (fun (i,_) -> i % 2 = odd)
    |> Seq.map snd

let partB inp =
    let santa     = mod2Index 0 inp |> locations
    let roboSanta = mod2Index 1 inp |> locations
    Set.union santa roboSanta |> Set.count

// --------------------------- Entry -------------------------------------------

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