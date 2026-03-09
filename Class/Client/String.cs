namespace PowerModule.Client;

using StringComparison = System.StringComparison;
using CultureInfo = System.Globalization.CultureInfo;

static internal class String
{
  internal const char Space = ' ';
  internal const char SingleQuote = '\'';
  internal const char DoubleQuote = '"';
  internal const string SingleQuoteString = "'";
  internal const string DoubleQuoteString = "\"";
  internal const string EscapedSingleQuote = "''";
  internal const string EscapedDoubleQuote = "\\\"";

  static internal CultureInfo CurrentCulture => currentCulture ??= CultureInfo.InvariantCulture;
  static private CultureInfo? currentCulture;

  static internal CultureInfo InvariantCulture => invariantCulture ??= CultureInfo.InvariantCulture;
  static private CultureInfo? invariantCulture;

  static internal string EscapeSingleQuoted(string text) => text.Contains(
    Space,
    System.StringComparison.Ordinal
  )
  || text.Contains(
    SingleQuote,
    System.StringComparison.Ordinal
  )
    ? string.Concat(
      SingleQuote,
      text.Replace(
        SingleQuoteString,
        EscapedSingleQuote,
        StringComparison.Ordinal
      ),
      SingleQuote
    )
    : text;

  static internal string EscapeDoubleQuoted(string text) => text.Contains(
      Space,
      StringComparison.Ordinal
    )
    ? string.Concat(
      DoubleQuote,
      text.Replace(
        DoubleQuoteString,
        EscapedDoubleQuote,
        StringComparison.Ordinal
      ),
      DoubleQuote
    )
    : text;

  static internal string UnescapeSingleQuoted(string escapedText) => escapedText is
  [
    SingleQuote,
    .. var text,
    SingleQuote,
  ]
    ? text.Replace(
      EscapedSingleQuote,
      SingleQuoteString,
      StringComparison.Ordinal
    )
    : escapedText;

  static internal string UnescapeDoubleQuoted(string escapedText) => escapedText is
  [
    DoubleQuote,
    .. var text,
    DoubleQuote,
  ]
    ? text.Replace(
      EscapedDoubleQuote,
      DoubleQuoteString,
      StringComparison.Ordinal
    )
    : escapedText;
}
