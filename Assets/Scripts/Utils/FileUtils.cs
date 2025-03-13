using System.IO;
using System.Text.RegularExpressions;

public static class FileUtils
{
    public static int ExtractNumber(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        Match match = Regex.Match(fileName, @"\d+"); // Extract digits

        return match.Success ? int.Parse(match.Value) : int.MaxValue;
    }
}
