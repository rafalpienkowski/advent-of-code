module AdventOfCode.Day15

let hash (input: string) : int =
    
    let rec calculate (current: string) (sum: int): int =
        if current.Length = 0 then
            sum
        else
            let charValue = int current[0]
            let newSum = ((sum + charValue) * 17) % 256
            
            calculate (current.Substring(1, current.Length - 1)) newSum
    
    calculate input 0