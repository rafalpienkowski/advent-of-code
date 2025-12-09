namespace Day3;

class Bank
{
    public long Joltage { get; private set; } = 0;
    public int BatteryCount { get; set; } = 2; // Number of batteries to select

    public void Load(IEnumerable<string> input)
    {
        foreach (var line in input)
        {
            Joltage += Find(line);
        }
    }

    private long Find(string line)
    {
        if (BatteryCount == 2)
        {
            // Part 1: Original logic for 2 batteries
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

            return int.Parse($"{first}{second}");
        }
        else
        {
            // Part 2: Select BatteryCount digits to maximize the number
            return long.Parse(FindMaxDigits(line, BatteryCount));
        }
    }

    private string FindMaxDigits(string line, int count)
    {
        int n = line.Length;
        var result = new System.Text.StringBuilder();
        int startIndex = 0;

        for (int i = 0; i < count; i++)
        {
            // For position i in result, we need (count - i) more digits total
            // We have (n - startIndex) digits remaining in input
            // So we can search in range: (n - startIndex) - (count - i) + 1
            int searchRange = n - startIndex - (count - i) + 1;

            char maxChar = '0';
            int maxIndex = startIndex;

            // Find the maximum digit in the valid search range
            for (int j = startIndex; j < startIndex + searchRange; j++)
            {
                if (line[j] > maxChar)
                {
                    maxChar = line[j];
                    maxIndex = j;
                }
            }

            result.Append(maxChar);
            startIndex = maxIndex + 1;
        }

        return result.ToString();
    }
}


public class Day3
{
    //[Fact]
    private void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(357, battery.Joltage);
    }

    //[Fact]
    private void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3b.txt");
        var battery = new Bank();
        battery.Load(lines);

        Assert.Equal(357, battery.Joltage);
    }

    //[Fact]
    private void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3.txt");
        var battery = new Bank();
        battery.BatteryCount = 12;
        battery.Load(lines);

        Assert.Equal(3121910778619, battery.Joltage);
    }

    //[Fact]
    private void Test4()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day3b.txt");
        var battery = new Bank();
        battery.BatteryCount = 12;
        battery.Load(lines);

        Assert.Equal(171039099596062, battery.Joltage);
    }
}
