namespace Day7;

class Laboratory
{
    public long Result { get; private set; } = 0;
    public long Timelines { get; private set; }
    char[][] labGrid;

    public void Load(IEnumerable<string> input)
    {
        for (var line = 0; line < input.Count(); line++)
        {
            var row = input.ElementAt(line).ToCharArray();
            labGrid = labGrid is null ? new char[input.Count()][] : labGrid;
            labGrid[line] = row;
        }

        for (var y = 1; y < labGrid.Length; y++)
        {
            for (var x = 0; x < labGrid[y].Length; x++)
            {
                if (labGrid[y - 1][x] == '|' || labGrid[y - 1][x] == 'S')
                {
                    if (labGrid[y][x] == '.')
                    {
                        labGrid[y][x] = '|';
                    }
                    if (labGrid[y][x] == '^')
                    {
                        if (x - 1 >= 0)
                        {
                            labGrid[y][x - 1] = '|';
                            Result++;
                        }
                        if (x + 1 < labGrid[y].Length)
                        {
                            labGrid[y][x + 1] = '|';
                            Result++;
                        }
                    }
                }

            }

            /*
            Console.WriteLine($"After processing row {y}:");
            PrintLab();
            Console.WriteLine($"Current Result: {Result}");
            Console.WriteLine("-------------------------");
            */
        }
    }

    private Dictionary<(int x, int y), long> memo = new();

    public void LoadPart2(IEnumerable<string> input)
    {
        for (var line = 0; line < input.Count(); line++)
        {
            var row = input.ElementAt(line).ToCharArray();
            labGrid = labGrid is null ? new char[input.Count()][] : labGrid;
            labGrid[line] = row;
        }

        int startX = -1, startY = -1;
        for (var y = 0; y < labGrid.Length; y++)
        {
            for (var x = 0; x < labGrid[y].Length; x++)
            {
                if (labGrid[y][x] == 'S')
                {
                    startX = x;
                    startY = y;
                    break;
                }
            }
            if (startX != -1) break;
        }

        memo.Clear();
        Timelines = CountTimelines(startX, startY + 1);
    }

    private long CountTimelines(int x, int y)
    {
        if (x < 0 || x >= labGrid[0].Length)
            return 0;

        if (y >= labGrid.Length)
            return 1;

        var key = (x, y);
        if (memo.TryGetValue(key, out var cached))
            return cached;

        long result;
        if (labGrid[y][x] == '^')
        {
            result = CountTimelines(x - 1, y + 1) + CountTimelines(x + 1, y + 1);
        }
        else
        {
            result = CountTimelines(x, y + 1);
        }

        memo[key] = result;
        return result;
    }

    private void PrintLab()
    {
        for (var y = 0; y < labGrid.Length; y++)
        {
            for (var x = 0; x < labGrid[y].Length; x++)
            {
                Console.Write(labGrid[y][x]);
            }
            Console.WriteLine();
        }
    }
}

public class Day7
{
    //[Fact]
    public void Test1()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7.txt");
        var lab = new Laboratory();
        lab.Load(input);

        Assert.Equal(21, lab.Result / 2);
    }

    //[Fact]
    public void Test2()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7b.txt");
        var lab = new Laboratory();
        lab.Load(input);

        Assert.Equal(1507, lab.Result / 2);
    }

    //[Fact]
    public void Test3()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7.txt");
        var lab = new Laboratory();
        lab.LoadPart2(input);

        Assert.Equal(40, lab.Timelines);
    }

    //[Fact]
    public void Test4()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7b.txt");
        var lab = new Laboratory();
        lab.LoadPart2(input);

        Assert.Equal(1537373473728, lab.Timelines);
    }
}
