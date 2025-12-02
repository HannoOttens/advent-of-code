open System.IO
open Shared.Shared
open FParsec

type Operator =
    SET of uint16
    | OR of string * string
    | AND of string * string
    | NOT of string
    | SHL of string * int
    | SHR of string * int
type Instruction = {
    op : Operator
    out : string
}

// --------------------------- Part A ------------------------------------------

let exec (map:Map<string,uint16>) op =
    match op with
        | SET n       -> n
        | NOT c1      -> ~~~map[c1]
        | OR  (c1,c2) -> map[c1] ||| map[c2]
        | AND (c1,c2) -> map[c1] &&& map[c2]
        | SHL (c1,n)  -> map[c1] <<< n
        | SHR (c1,n)  -> map[c1] >>> n

let execInstruction map inst = Map.add inst.out (exec map inst.op) map
let partA = Seq.fold execInstruction Map.empty

// --------------------------- Part B ------------------------------------------

// let partB insts = instructionToField comd2funcB insts |> Seq.sum

// --------------------------- Parse -------------------------------------------

let letters = many1Chars letter

let parseOp =
    puint16 |>> (fun n -> SET n)
    <|> (pstring "NOT " >>. letters |>> fun c1 -> NOT c1)
    <|> attempt (pipe2 (letters .>> pstring " AND ") letters (fun c1 c2 -> AND (c1,c2)))
    <|> attempt (pipe2 (letters .>> pstring " OR " ) letters (fun c1 c2 -> OR (c1,c2)))
    <|> attempt (pipe2 (letters .>> pstring " LSHIFT ") pint32 (fun c n -> SHL (c,n)))
    <|> attempt (pipe2 (letters .>> pstring " RSHIFT ") pint32 (fun c n -> SHR (c,n)))

let parseLine =
    pipe2 (parseOp .>> pstring " -> ") letters
            (fun op out -> { op = op; out = out })
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
    let map = partA parsed
    printf "%d" map["a"]
    0