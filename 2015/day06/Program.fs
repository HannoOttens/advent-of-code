open System.IO
open Shared.Shared
open FParsec


type Command = On | Off | Toggle

type Instruction = {
    comd : Command
    p1 : int * int
    p2 : int * int
}
let gridSize = 1000

// --------------------------- Part A ------------------------------------------

let processInstruction comd2func (flags:int array) inst =
    let func = comd2func inst.comd
    for y in snd inst.p1..snd inst.p2 do
        for x in fst inst.p1..fst inst.p2 do
            let pos = y * gridSize + x
            flags[pos] <- func flags[pos]
    flags

let instructionToField comd2func =
    Seq.fold (processInstruction comd2func)
        [| for _ in 0..gridSize*gridSize -> 0 |]

let comd2funcA comd =
    match comd with
        | On     -> fun _ -> 1
        | Off    -> fun _ -> 0
        | Toggle -> fun v -> if v = 1 then 0 else 1

let partA insts = instructionToField comd2funcA insts |> Seq.sum

// --------------------------- Part B ------------------------------------------

let comd2funcB comd =
    match comd with
        | On     -> fun v -> v + 1
        | Off    -> fun v -> max 0 (v - 1)
        | Toggle -> fun v -> v + 2

let partB insts = instructionToField comd2funcB insts |> Seq.sum

// --------------------------- Parse -------------------------------------------

let parseCoord = pint32 .>> pchar ',' .>>. pint32
let parseCommand =
        (pstring "toggle "   >>% Toggle)
    <|> (pstring "turn on " >>% On)
    <|> (pstring "turn off "  >>% Off)

let parseLine =
    pipe3 parseCommand (parseCoord .>> pstring " through ") parseCoord
          (fun c p1 p2 -> { comd = c; p1 = p1; p2 = p2 });

let parseLines = many (parseLine .>> (newline >>% () <|> eof))

let parse input =
    match run parseLines input with
        | Success(res,_,_) -> res
        | Failure(err,_,_) -> failwith err

// --------------------------- Entry -------------------------------------------

[<EntryPoint>]
let main argv =
    let input = File.ReadAllText "input.txt"
    let parsed = parse input
    let total =
            partB parsed
    printf "%d" total
    0