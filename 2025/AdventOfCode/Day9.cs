namespace Day9;

record Point(int X, int Y);

class Floor
{
    private List<Point> allPoints = new();
    private List<Point> candidates = new();
    public int Rectangle { get; private set; } = 0;

    public void Load(List<string> input)
    {
        foreach (var line in input)
        {
            var parts = line.Split(",", StringSplitOptions.TrimEntries);
            allPoints.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
        }

        allPoints = allPoints.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
        candidates = GrahamScan(allPoints);

        //Print(candidates);
        CalculateRectangle();
    }

    private void CalculateRectangle()
    {
        for (int i = 0; i < allPoints.Count; i++)
        {
            for (int j = i + 1; j < allPoints.Count; j++)
            {
                var p1 = allPoints[i];
                var p2 = allPoints[j];

                int width = Math.Abs(p2.X - p1.X) + 1;
                int height = Math.Abs(p2.Y - p1.Y) + 1;
                int area = width * height;

                if (area > Rectangle)
                {
                    Rectangle = area;
                }
            }
        }
    }

    private void Print(List<Point> points)
    {
        int maxX = points.Max(p => p.X);
        int maxY = points.Max(p => p.Y);

        for (int y = 0; y < maxY + 2; y++)
        {
            for (int x = 0; x < maxX + 2; x++)
            {
                if (points.Contains(new Point(x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    private List<Point> GrahamScan(List<Point> points)
    {
        if (points.Count <= 1)
            return points;

        points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

        List<Point> lower = new();
        foreach (var p in points)
        {
            while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
            {
                lower.RemoveAt(lower.Count - 1);
            }
            lower.Add(p);
        }

        List<Point> upper = new();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            var p = points[i];
            while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
            {
                upper.RemoveAt(upper.Count - 1);
            }
            upper.Add(p);
        }

        upper.RemoveAt(upper.Count - 1);
        lower.RemoveAt(lower.Count - 1);
        lower.AddRange(upper);
        return lower;
    }

    private int Cross(Point o, Point a, Point b)
    {
        return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
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

    [Fact]
    public void Test2()
    {
        var input = AdventOfCode.Utils.ReadInputLines("day9b.txt");
        var floor = new Floor();
        floor.Load(input);

        Assert.Equal(50, floor.Rectangle);
    }
}
