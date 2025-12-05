namespace Day5;

class Fresher
{
    public long Number { get; private set; } = 0;
    public long Total { get; private set; } = 0;
    private List<List<long>> freshNumbers = new();

    public void Load(IEnumerable<string> input)
    {
        var fresh = true;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                fresh = false;

                CaclTotal();

                continue;
            }

            if (fresh)
            {
                var numbers = line.Split('-').Select(long.Parse).ToList();
                freshNumbers.Add(numbers);
            }
            else
            {
                if (IsFresh(line))
                {
                    Number++;
                }
            }
        }
    }

    private void CaclTotal()
    {
        if (freshNumbers.Count == 0)
        {
            Total = 0;
            return;
        }

        var sortedRanges = freshNumbers.OrderBy(r => r[0]).ToList();

        var mergedRanges = new List<List<long>>();
        var currentRange = new List<long> { sortedRanges[0][0], sortedRanges[0][1] };

        for (int i = 1; i < sortedRanges.Count; i++)
        {
            var nextRange = sortedRanges[i];

            if (nextRange[0] <= currentRange[1] + 1)
            {
                currentRange[1] = Math.Max(currentRange[1], nextRange[1]);
            }
            else
            {
                mergedRanges.Add(currentRange);
                currentRange = new List<long> { nextRange[0], nextRange[1] };
            }
        }

        mergedRanges.Add(currentRange);

        Total = mergedRanges.Sum(r => r[1] - r[0] + 1);
    }

    private bool IsFresh(string value)
    {
        var number = long.Parse(value);
        foreach (var list in freshNumbers)
        {
            //Console.WriteLine($"Checking {number} against range {list[0]}-{list[1]}");
            if (number >= list[0] && number <= list[1])
            {
                return true;
            }
        }
        return false;
    }
}

public class Day5
{
    //[Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day5.txt");
        var fresher = new Fresher();
        fresher.Load(lines);

        Assert.Equal(3, fresher.Number);
    }

    //[Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day5b.txt");
        var fresher = new Fresher();
        fresher.Load(lines);

        Assert.Equal(3, fresher.Number);
    }

    [Fact]
    public void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day5.txt");
        var fresher = new Fresher();
        fresher.Load(lines);

        Assert.Equal(14, fresher.Total);
    }

    [Fact]
    public void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day5b.txt");
        var fresher = new Fresher();
        fresher.Load(lines);

        Assert.Equal(345755049374932, fresher.Total);
    }
}
