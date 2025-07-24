open System.IO
open Shared.Shared

// --------------------------- Part A ------------------------------------------

let isVowel char = List.contains char ['a'; 'e'; 'i'; 'o'; 'u']
let isNaugthy (a,b) = List.contains (string a + string b) ["ab";"cd";"pq";"xy"]

let vowels = Seq.filter isVowel >> Seq.length

let doubleChars dist str =
    str
    |> Seq.skip dist
    |> Seq.zip str
    |> Seq.exists (fun (a,b) -> a = b)

let naughtyPairs str =
    Seq.skip 1 str |> Seq.zip str |> Seq.exists isNaugthy

let isNice str =
    vowels str >= 3
    && doubleChars 1 str
    && not (naughtyPairs str)

let partA = Seq.filter isNice >> Seq.length

// --------------------------- Part B ------------------------------------------

let containsAfter (line:string) i (pair:string) =
    line[i..].Contains pair

let twoPair (line:string) =
    Seq.skip 1 line
    |> Seq.zip line
    |> Seq.mapi (fun i (a,b) -> i+2, string a + string b)
    |> Seq.exists (fun (i,pair) -> containsAfter line i pair)

let partB = Seq.filter (fun str -> doubleChars 2 str && twoPair str) >> Seq.length

// --------------------------- Entry -------------------------------------------

[<EntryPoint>]
let main argv =
    let input = File.ReadAllLines "input.txt"
    let total =
        if isPartA argv then
            partA input
        else
            partB input
    printf "%d" total
    0