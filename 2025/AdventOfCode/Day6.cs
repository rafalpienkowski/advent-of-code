namespace Day6;

class Homeworker
{
    public long Result { get; private set; } = 0;
    public long Result2 { get; private set; } = 0;
    private List<List<long>> homeworkAssignments = new();
    private List<string> operations = new();

    public void Load2(IEnumerable<string> input)
    {
        var inputList = input.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
        if (inputList.Count == 0) return;

        int rows = inputList.Count;
        int cols = inputList[0].Length;
        char[,] matrix = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string line = inputList[i];
            for (int j = 0; j < line.Length && j < cols; j++)
            {
                matrix[i, j] = line[j];
            }
        }

        List<(int start, int end)> problems = new List<(int, int)>();
        int problemStart = -1;

        for (int col = 0; col < cols; col++)
        {
            bool isSpaceColumn = true;
            for (int row = 0; row < rows; row++)
            {
                if (matrix[row, col] != ' ')
                {
                    isSpaceColumn = false;
                    break;
                }
            }

            if (isSpaceColumn)
            {
                if (problemStart != -1)
                {
                    problems.Add((problemStart, col - 1));
                    problemStart = -1;
                }
            }
            else
            {
                if (problemStart == -1)
                {
                    problemStart = col;
                }
            }
        }

        if (problemStart != -1)
        {
            problems.Add((problemStart, cols - 1));
        }

        foreach (var (start, end) in problems)
        {
            List<long> numbers = new List<long>();
            char? operation = null;

            for (int col = end; col >= start; col--)
            {
                string columnValue = "";
                for (int row = 0; row < rows; row++)
                {
                    char c = matrix[row, col];
                    if (c != ' ')
                    {
                        if (c == '+' || c == '*')
                        {
                            operation = c;
                        }
                        else if (char.IsDigit(c))
                        {
                            columnValue += c;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(columnValue))
                {
                    numbers.Add(long.Parse(columnValue));
                }
            }

            if (numbers.Count > 0 && operation.HasValue)
            {
                long result = numbers[0];
                for (int i = 1; i < numbers.Count; i++)
                {
                    if (operation == '+')
                    {
                        result += numbers[i];
                    }
                    else if (operation == '*')
                    {
                        result *= numbers[i];
                    }
                }
                Result2 += result;
            }
        }
    }

    public void Load(IEnumerable<string> input)
    {
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var segment = line.Trim().Split(' ').First();
            if (long.TryParse(segment, out _))
            {
                var assignments = line.Split(' ').ToList();

                var assigmntmentList = new List<long>();
                foreach (var assignment in assignments)
                {
                    if (string.IsNullOrEmpty(assignment))
                    {
                        continue;
                    }
                    assigmntmentList.Add(long.Parse(assignment));
                }

                homeworkAssignments.Add(assigmntmentList);
            }
            else
            {
                var operationList = new List<string>();
                var op = line.Split(' ').ToList();
                foreach (var operation in op)
                {
                    if (string.IsNullOrEmpty(operation))
                    {
                        continue;
                    }
                    operations.Add(operation);
                }
            }
        }

        for (var i = 0; i < operations.Count; i++)
        {
            var op = operations[i];
            if (op == "+")
            {
                long tmp = 0;
                for (var j = 0; j < homeworkAssignments.Count; j++)
                {
                    tmp += homeworkAssignments[j][i];
                }

                Result += tmp;
            }
            else if (op == "*")
            {
                long tmp = 1;
                for (var j = 0; j < homeworkAssignments.Count; j++)
                {
                    tmp *= homeworkAssignments[j][i];
                }

                Result += tmp;
            }
        }
    }
}

public class Day6
{
    //[Fact]
    private void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day6.txt");
        var homeworker = new Homeworker();
        homeworker.Load(lines);

        Assert.Equal(4277556, homeworker.Result);
    }

    //[Fact]
    private void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day6b.txt");
        var homeworker = new Homeworker();
        homeworker.Load(lines);

        Assert.Equal(4277556, homeworker.Result);
    }

    //[Fact]
    private void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day6.txt");
        var homeworker = new Homeworker();
        homeworker.Load2(lines);

        Assert.Equal(3263827, homeworker.Result2);
    }

    //[Fact]
    private void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day6b.txt");
        var homeworker = new Homeworker();
        homeworker.Load2(lines);

        Assert.Equal(10194584711842, homeworker.Result2);
    }
}
