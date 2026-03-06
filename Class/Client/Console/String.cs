namespace Module.Client.Console;

internal static class String
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

  internal static string UnescapeSingleQuoted(string escapedText) => escapedText is
  [
    SingleQuote,
    .. var text,
    SingleQuote,
  ]
    ? text.Replace(
      EscapedSingleQuote,
      SingleQuoteString
    )
    : escapedText;

  internal static string UnescapeDoubleQuoted(string escapedText) => escapedText is
  [
    DoubleQuote,
    .. var text,
    DoubleQuote,
  ]
    ? text.Replace(
      EscapedDoubleQuote,
      DoubleQuoteString
    )
    : escapedText;
}
