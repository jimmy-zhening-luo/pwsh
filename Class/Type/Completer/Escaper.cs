using System.Management.Automation.Language;

namespace Completer
{
  public static class Escaper
  {
    public static string Unescape(string escapedText) => (
      escapedText.Length > 1
      && escapedText.StartsWith('\'')
      && escapedText.EndsWith('\'')
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
  } // class Escaper
} // namespace Completer
