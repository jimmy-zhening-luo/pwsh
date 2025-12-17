using System.Management.Automation.Language;

namespace Completer
{
  namespace Typed
  {
    public static class Typed
    {
      public readonly static char SpaceChar = ' ';
      public readonly static char QuoteChar = '\'';
      public readonly static string Space = " ";
      public readonly static string Quote = "'";
      public readonly static string EscapedQuote = "''";
  
      public static string Unescape(string escapedText)
      {
        return (
          escapedText.Length > 1
          && escapedText.StartsWith(QuoteChar)
          && escapedText.EndsWith(QuoteChar)
        )
          ? escapedText[1..^1]
            .Replace(
              EscapedQuote,
              Quote
            )
          : escapedText;
      }
  
      public static string Escape(string text)
      {
        return text.Contains(SpaceChar)
          ? Quote
            + CodeGeneration.EscapeSingleQuotedStringContent(text)
            + Quote
          : text;
      }
    }
  }
}
