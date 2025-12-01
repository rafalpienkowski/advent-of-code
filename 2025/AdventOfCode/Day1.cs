namespace Day1;


public class Safe
{
    public int Position { get; private set; } = 50;
    public int Zeros { get; private set; } = 0;

    public void ReadLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var direction = line[0];
            var move = int.Parse(line[1..]);

            for (int i = 0; i < move; i++)
            {
                if (direction == 'L')
                {
                    Position = (Position - 1 + 100) % 100;
                }
                else if (direction == 'R')
                {
                    Position = (Position + 1) % 100;
                }

                if (Position == 0)
                {
                    Zeros++;
                }
            }
        }
    }
}


public class Day1
{
    private readonly Safe _safe = new Safe();

    //[Fact]
    public void Test1()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day1.txt");
        _safe.ReadLines(lines);

        Assert.Equal(3, _safe.Zeros);
    }

    //[Fact]
    public void Test2()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day1b.txt");
        _safe.ReadLines(lines);

        Assert.Equal(1195, _safe.Zeros);
    }

    [Fact]
    public void Test3()
    {
        var lines = AdventOfCode.Utils.ReadInputLines("day1.txt");
        _safe.ReadLines(lines);

        Assert.Equal(6, _safe.Zeros);
    }

    [Fact]
    public void Test4(){
        var lines = AdventOfCode.Utils.ReadInputLines("day1b.txt");
        _safe.ReadLines(lines);

        Assert.Equal(20063, _safe.Zeros);
    }
}
