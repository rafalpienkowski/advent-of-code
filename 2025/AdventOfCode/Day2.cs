namespace Day2;

class Finder
{

    public long Sum { get; private set; } = 0;

    public void Load(string line)
    {
        var parts = line.Split(',');
        foreach (var part in parts)
        {
            var rangeParts = part.Split('-');
            var start = long.Parse(rangeParts[0]);
            var end = long.Parse(rangeParts[1]);

            for (long i = start; i <= end; i++)
            {
                if (IsRepeatedDigit(i))
                {
                    Sum += i;
                }
            }
        }
    }

    private bool IsRepeatedDigit(long number)
    {
        var str = number.ToString();

        if (str.Length % 2 != 0)
            return false;

        var first = str[..(str.Length / 2)];
        var second = str[(str.Length / 2)..];

        return first == second;
    }
}

public class Day2
{
    [Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(1227775554, finder.Sum);
    }

    [Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2b.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(37314786486, finder.Sum);
    }
}
