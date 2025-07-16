open System
open System.IO
open Shared.Shared
open System.Security.Cryptography

// --------------------------- Part A ------------------------------------------

let hash str =
    str
    |> Seq.map byte
    |> Seq.toArray
    |> MD5.Create().ComputeHash

let testValue b3Mask inp n =
    let md5 = hash (inp + string n)
    match Array.truncate 3 md5 with
        | [|b1; b2; b3|] -> b1              = 0uy
                         && b2              = 0uy
                         && b3 &&& b3Mask = 0uy
        | _ -> failwith "MD5 hashes really are longer"

let rec partA inp n =
    if testValue 0b11110000uy inp n  then
        n
    else
        partA inp (n+1)

// --------------------------- Part B ------------------------------------------

let rec partB inp n =
    if testValue 0b11111111uy inp n  then
        n
    else
        partB inp (n+1)

// --------------------------- Entry -------------------------------------------

[<EntryPoint>]
let main argv =
    let input = File.ReadAllText "input.txt"
    let total =
        if isPartA argv then
            partA input 0
        else
            partB input 0
    printf "%d" total
    0