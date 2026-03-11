namespace PowerModule.Client;

using StringComparison = System.StringComparison;
using CultureInfo = System.Globalization.CultureInfo;

static class StringInput
{
  internal const char Space = ' ';
  internal const char SingleQuote = '\'';
  internal const char DoubleQuote = '"';
  internal const string StringSingleQuote = "'";
  internal const string StringDoubleQuote = "\"";
  internal const string EscapedSingleQuote = "''";
  internal const string EscapedDoubleQuote = "\\\"";

  static internal CultureInfo CurrentCulture => currentCulture ??= CultureInfo.InvariantCulture;
  static CultureInfo? currentCulture;

  static internal CultureInfo InvariantCulture => invariantCulture ??= CultureInfo.InvariantCulture;
  static CultureInfo? invariantCulture;

  static internal string EscapeSingleQuoted(string text) => IsUnsafe(text)
    ? string.Concat(
      SingleQuote,
      text.Replace(
        StringSingleQuote,
        EscapedSingleQuote,
        StringComparison.Ordinal
      ),
      SingleQuote
    )
    : text;

  static internal string EscapeDoubleQuoted(string text) => IsUnsafe(text)
    ? string.Concat(
      DoubleQuote,
      text.Replace(
        StringDoubleQuote,
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
      StringSingleQuote,
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
      StringDoubleQuote,
      StringComparison.Ordinal
    )
    : escapedText;

  static bool IsUnsafe(string text) => text.Contains(
    Space,
    StringComparison.Ordinal
  )
  || text.Contains(
    SingleQuote,
    StringComparison.Ordinal
  )
  || text.Contains(
    DoubleQuote,
    StringComparison.Ordinal
  );
}
