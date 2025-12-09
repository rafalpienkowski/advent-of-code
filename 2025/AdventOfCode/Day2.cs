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
                if (IsRepeatedAtLeastTwice(i))
                {
                    Console.WriteLine($"Adding {i} to sum");
                    Sum += i;
                }
            }
        }
    }

    private bool IsRepeatedAtLeastTwice(long number)
    {
        var str = number.ToString();
        var length = str.Length;

        for (int size = 1; size <= length / 2; size++)
        {
            int start = 0;
            var first = str.Substring(start, size);
            if (first == "0")
                continue;

            var second = str.Substring(start + size, size);

            if (first == second)
            {
                if (first.Length + second.Length < length)
                {

                    var leftover = str.Substring(first.Length + second.Length);
                    if (IsRepeated(first, leftover))
                    {
                        Console.WriteLine($"Found repeated sequence {first} in number {number}");
                        return true;
                    }

                }
                else
                {
                    Console.WriteLine($"Found repeated sequence {first} in number {number}");
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsRepeated(string first, string leftover)
    {
        if (leftover.Substring(0, Math.Min(first.Length, leftover.Length)) == first)
        {
            if (first.Length == leftover.Length)
            {
                Console.WriteLine($"Found repeated sequence {first}, leftover {leftover} (full match)");
                return true;
            }
            else
            {
                if (IsRepeated(first, leftover.Substring(first.Length)))
                {
                    return true;
                }
            }
        }
        return false;
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
    //[Fact]
    private void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(1227775554, finder.Sum);
    }

    //[Fact]
    private void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2b.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(37314786486, finder.Sum);
    }

    //[Fact]
    private void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(4174379265, finder.Sum);
    }

    //[Fact]
    private void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day2b.txt");
        var line = lines.First();
        var finder = new Finder();
        finder.Load(line);

        Assert.Equal(4174379265, finder.Sum);
    }
}
