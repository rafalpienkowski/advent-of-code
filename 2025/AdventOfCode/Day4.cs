namespace Day4;

class Grid
{
    public int Rolls { get; private set; } = 0;

    private char[][] grid;

    public void Load(List<string> input)
    {
        var size = input.Count;
        grid = new char[size][];

        for (int i = 0; i < size; i++)
        {
            grid[i] = input[i].ToCharArray();
        }

        for (var x = 0; x < size; x++)
        {
            for (var y = 0; y < size; y++)
            {
                //Console.WriteLine($"Checking {x} {y}");
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
                //Console.WriteLine($"X: {x}, Y: {y}, tmp rolls: {tmpRolls}, rolls {Rolls}");

                if (tmpRolls <= 4)
                {
                    Rolls++;
                }
            }
        }
    }
}

public class Day4
{
    [Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(13, grid.Rolls);
    }

    [Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day4b.txt");
        var grid = new Grid();
        grid.Load(lines);

        Assert.Equal(1474, grid.Rolls);
    }
}
