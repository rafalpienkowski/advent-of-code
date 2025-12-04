namespace Day4;

record Point(int X, int Y);

class Grid
{
    public List<Point> Rolls { get; set; } = [];
    public int TotalRolls { get; set; } = 0;

    private char[][] grid;

    public void Load(List<string> input)
    {
        var size = input.Count;
        grid = new char[size][];

        for (int i = 0; i < size; i++)
        {
            grid[i] = input[i].ToCharArray();
        }

        do
        {
            Rolls = [];
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (grid[y][x] != '@')
                    {
                        continue;
                    }

                    var tmpRolls = 0;

                    for (var k = x - 1; k <= x + 1; k++)
                    {
                        for (var l = y - 1; l <= y + 1; l++)
                        {
                            if (k < 0 || l < 0 || k >= grid[0].Length || l >= grid.Length)
                            {
                                continue;
                            }

                            if (grid[l][k] == '@')
                            {
                                tmpRolls++;
                            }
                        }
                    }
                    if (tmpRolls <= 4)
                    {
                        //Console.WriteLine($"Rolling at {x},{y} with {tmpRolls} adjacent rolls.");
                        Rolls.Add(new Point(x, y));
                    }
                }
            }

            TotalRolls += Rolls.Count;
            foreach (var point in Rolls)
            {
                grid[point.Y][point.X] = '.';
            }
        } while (Rolls.Count() > 0);
    }
}

public class Day4
{
    //[Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(13, grid.Rolls.Count);
    }

    //[Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4b.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(1474, grid.Rolls.Count);
    }

    [Fact]
    public void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(43, grid.TotalRolls);
    }

    [Fact]
    public void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4b.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(8910, grid.TotalRolls);
    }
}
