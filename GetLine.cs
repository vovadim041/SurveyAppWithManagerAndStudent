namespace SurveyApp;

public static class GetLine
{
    public static string Get(string fileName, int line)
    {
        using var streamReader = new StreamReader(fileName);
        for (var i = 1; i < line; i++)
            streamReader.ReadLine();
        return streamReader.ReadLine() ?? string.Empty;
    }
}