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

/// Matrix of question marks.
/// Each element is a list of the individual question marks for one student
/// In the same order as found in markHeaders
let markMatrix =
    marks
    |> List.mapi ( fun i row ->
        markHeaders
        |> List.map (getMark i)
        |> (fun row -> if List.forall Option.isSome row then [ List.map Option.get row] else []))
    |> List.concat

let getRow i = markMatrix[i]

/// List of all the marks awarded for question i
let qMarks i = 
    markMatrix
    |> List.map (List.item i)

let qMarkBins i =
    List.groupBy id (qMarks i)
    |> List.map (fun (bin, cases ) -> bin, List.length cases)
    |> List.sortDescending
    |> (function | bins when bins.Length = 2 -> [] | bins -> bins)

let displayBin (bin,num) =
    $"%.2f{bin}->%-3d{num}"
let qIndexes = [0..List.length markHeaders - 1]

/// list of each student's mark total
let testMarks =
    markMatrix
    |> List.map List.sum

/// PD of ith question
let discriminate i = Correlation.Seq.pearson testMarks (qMarks i)
let maxMark i = List.max (qMarks i)

/// maximum mark awarded for ith question (should be 1 normally)
let maxMarks = 
    [0..markMatrix[0].Length-1] |> List.map (fun i -> maxMark i)

/// average mark for test over all students
let testAverage = (List.average testMarks / float (List.length maxMarks))

/// average mark for ith question over all students
let average i = List.average (qMarks i)

/// print stats about question marks
let analyse() =
    printfn $"Average test score (%%) = %.2f{testAverage*100.}"
    printfn "        PD     Av   Max   Histogram"
    markHeaders
    |> List.iteri (fun i h ->
        let sep = "  "
        printfn $"Q%-5s{h} %.2f{discriminate i} %5.2f{average i} %5.2f{maxMark i}   \
        %s{List.map displayBin (qMarkBins i) |> String.concat sep}")

analyse()