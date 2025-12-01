namespace Day1;


public class Safe
{
    public int Position { get; private set; } = 50;
    public int Zeros { get; private set; } = 0;

    public void ReadLines(IEnumerable<string> lines)
    {
        var idx = 0;
        foreach (var line in lines)
        {
            if (idx++ > 400)
            {
                //return;
            }
            var direction = line[0];
            var move = int.Parse(line[1..]);

            if (direction == 'L')
            {
                Position = (Position - move + 100) % 100;
            }
            else if (direction == 'R')
            {
                Position = (Position + move) % 100;
            }

            if (Position == 0)
            {
                Zeros++;
            }

            //Console.WriteLine($"The dial is rotated: {line} to point at {Position}");
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

    //[Fact]
    public void Test4(){
        var lines = AdventOfCode.Utils.ReadInputLines("day1b.txt");
        _safe.ReadLines(lines);

        Assert.Equal(20063, _safe.Zeros);
    }
}
