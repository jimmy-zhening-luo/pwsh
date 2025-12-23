using System.Management.Automation.Language;

namespace Completer
{
  public static class Escaper
  {
    public static string Unescape(string escapedText) => (
      escapedText.Length > 1
      && escapedText.StartsWith('\u0027')
      && escapedText.EndsWith('\u0027')
    )
      ? escapedText[1..^1]
        .Replace(
          "''",
          "'"
        )
      : escapedText;

    public static string Escape(string text) => text.Contains(' ')
      ? "'"
        + CodeGeneration.EscapeSingleQuotedStringContent(text)
        + "'"
      : text;
  }
}
