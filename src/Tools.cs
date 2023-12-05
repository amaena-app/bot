
public class Tools
{
    public static string SanitizeString(string? original, bool keepNewLines = true)
    {
        if (original != null)
            return original.Replace("\n", keepNewLines ? "\\n" : "").Replace("\r", keepNewLines ? "\\r" : "").Replace("\"", "\\\"");
        else
            return "";
    }
}