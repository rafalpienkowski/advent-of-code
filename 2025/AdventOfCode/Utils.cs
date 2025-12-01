namespace AdventOfCode;

public static class Utils
{
    public static IEnumerable<string> ReadInputLines(string fileName)
    {
        return File.ReadLines(Path.Combine("input", fileName));
    }
}
