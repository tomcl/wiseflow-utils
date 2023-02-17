open FSharp.Interop.Excel
open FSharp.Stats


// Let the type provider do it's work
type Marks = ExcelFile<"../WFlow.xlsx", ForceString=true>
type MarksHeader = ExcelFile<"../WFlow.xlsx", HasHeaders=false,ForceString=true>
let marks = (new Marks()).Data |> Seq.toList |> List.tail
let header = (new MarksHeader()).Data |> Seq.toList |> List.head
let getHeaderName (n:int) = header.GetValue n
let hasHeader (n:int) = header.GetValue n <> null
let headers : string list =
    [0..25]
    |> List.takeWhile hasHeader 
    |> List.map (fun n -> (unbox (header.GetValue n)))


let getMark row (col:string) : float option=
    List.tryItem row marks
    |> Option.bind (fun row -> 
        System.Single.TryParse(sprintf $"{row.GetValue col}")
        |> function | true, d -> Some (float d) | _ -> None)
                     

let markHeaders = 
    headers
    |> List.filter (fun h -> h.Contains(":"))   

let markMatrix =
    marks
    |> List.mapi ( fun i row ->
        markHeaders
        |> List.map (getMark i)
        |> (fun row -> if List.forall Option.isSome row then [ List.map Option.get row] else []))
    |> List.concat



let getRow i = markMatrix[i]

let qMarks i = 
    markMatrix
    |> List.map (List.item i)



let testMarks =
    markMatrix
    |> List.map List.sum



let discriminate i = Correlation.Seq.pearson testMarks (qMarks i)

let analyse() =
    markHeaders
    |> List.mapi (fun i h ->
        printfn $"Q%-5s{h} %.2f{discriminate i}")

analyse()