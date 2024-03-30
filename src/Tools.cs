
using System.Globalization;
using System.Text.RegularExpressions;

public class Tools
{
    public static string SanitizeString(string? original, bool keepNewLines = true)
    {
        if (original != null)
            return original.Replace("\n", keepNewLines ? "\\n" : "").Replace("\r", keepNewLines ? "\\r" : "").Replace("\"", "\\\"");
        else
            return "";
    }

    public static string DecodeEncodedNonAsciiCharacters( string value ) {
        return Regex.Replace(
            value,
            @"\\u(?<Value>[a-zA-Z0-9]{4})",
            m => {
                return ((char) int.Parse( m.Groups["Value"].Value, NumberStyles.HexNumber )).ToString();
            } );
    }

    public static double ROME_LAT = 41.8992;

    public static double ROME_LON = 12.5450;

    public static bool VERBOSE = false;

}