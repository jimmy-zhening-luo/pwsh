namespace Module.Client.Console;

internal static partial class String
{
  internal const char Space = ' ';
  internal const char SingleQuote = '\'';
  internal const char DoubleQuote = '"';
  internal const string SingleQuoteString = "'";
  internal const string DoubleQuoteString = "\"";
  internal const string EscapedSingleQuote = "''";
  internal const string EscapedDoubleQuote = "\\\"";

  internal static string EscapeSingleQuoted(string text) => text.Contains(Space)
    ? string.Concat(
        SingleQuote,
        System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(text),
        SingleQuote
      )
    : text;

  internal static string EscapeDoubleQuoted(string text) => text.Contains(Space)
  ? string.Concat(
      DoubleQuote,
      text.Replace(
        DoubleQuoteString,
        EscapedDoubleQuote
      ),
      DoubleQuote
    )
  : text;

  internal static string UnescapeSingleQuoted(string escapedText) => Unescape(
    escapedText,
    SingleQuote,
    EscapedSingleQuote,
    SingleQuoteString
  );

  internal static string UnescapeDoubleQuoted(string escapedText) => Unescape(
    escapedText,
    DoubleQuote,
    EscapedDoubleQuote,
    DoubleQuoteString
  );

  private static string Unescape(
    string escapedText,
    char quote,
    string escapedQuote,
    string quoteString
  ) => (
    escapedText.Length > 1
    && escapedText.StartsWith(quote)
    && escapedText.EndsWith(quote)
  )
    ? escapedText[1..^1].Replace(
        escapedQuote,
        quoteString
      )
    : escapedText;
}
