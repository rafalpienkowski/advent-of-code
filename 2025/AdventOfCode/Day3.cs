namespace Day3;

class Bank
{
    public long Joltage { get; private set; } = 0;

    public void Load(IEnumerable<string> input)
    {
        foreach (var line in input)
        {
            Joltage += Find(line);
        }
    }

    private int Find(string line)
    {
        int first = 0;
        int second = 0;
        for (int idx = 0; idx < line.Length - 1; idx++)
        {
            int tmp = int.Parse(line.Substring(idx, 1));
            int tmp2 = 0;
            //check last 
            if (idx == line.Length - 2)
            {
                tmp2 = int.Parse(line.Substring(idx + 1, 1));
            }

            if (tmp > first)
            {
                first = tmp;
                second = tmp2;
                continue;
            }

            var last = tmp > tmp2 ? tmp : tmp2;
            if (last > second)
            {
                second = last;
                continue;
            }

        }

        var result = int.Parse($"{first}{second}");

        return result;
    }
}


public class Day3
{
    //[Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(357, battery.Joltage);
    }

    //[Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3b.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(357, battery.Joltage);
    }

    [Fact]
    public void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(3121910778619, battery.Joltage);
    }

    //[Fact]
    public void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3b.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(357, battery.Joltage);
    }
}
