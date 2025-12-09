namespace Day9;

record Point(int X, int Y);

class Floor
{
    public List<Point> Points = new();
    public int Rectangle { get; private set; } = 0;

    public void Load(List<string> input)
    {
        foreach (var line in input)
        {
            var parts = line.Split(",", StringSplitOptions.TrimEntries);
            Points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
        }
    }
}

public class Day9
{
    [Fact]
    public void Test1()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day9.txt");
        var floor = new Floor();
        floor.Load(input);

        Assert.Equal(50, floor.Rectangle);
    }
}
