using System;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;

namespace Input
{
  public static class Text
  {
    public readonly static char Space = ' ';
    public readonly static char Quote = '\'';
    public readonly static string SpaceString = " ";
    public readonly static string QuoteString = "'";
    public readonly static string EscapedQuoteString = "''";

    public static string Unescape(string escapedText)
    {
      return (
        escapedText.Length > 1
        && escapedText.StartsWith(Quote)
        && escapedText.EndsWith(Quote)
      )
        ? escapedText[1..^1]
          .Replace(
            EscapedQuoteString,
            QuoteString
          )
        : escapedText;
    }

    public static string Escape(string text)
    {
      return text.Contains(Space)
        ? QuoteString
          + CodeGeneration.EscapeSingleQuotedStringContent(text)
          + QuoteString
        : text;
    }
  }
}
