namespace Day7;

class Laboratory
{
    public long Result { get; private set; } = 0;
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
    [Fact]
    public void Test1()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7.txt");
        var lab = new Laboratory();
        lab.Load(input);

        Assert.Equal(21, lab.Result / 2);
    }

    [Fact]
    public void Test2()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day7b.txt");
        var lab = new Laboratory();
        lab.Load(input);

        Assert.Equal(1507, lab.Result / 2);
    }
}
