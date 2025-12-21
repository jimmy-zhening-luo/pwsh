using System.Management.Automation.Language;

namespace Completer
{
  public static class Stringifier
  {
    public static string Unescape(string escapedText)
    {
      return (
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
    }

    public static string Escape(string text)
    {
      return text.Contains(' ')
        ? "'"
          + CodeGeneration.EscapeSingleQuotedStringContent(text)
          + "'"
        : text;
    }
  } // class Stringifier
} // namespace Completer
