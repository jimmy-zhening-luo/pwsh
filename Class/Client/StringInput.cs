namespace PowerModule.Client;

using CultureInfo = System.Globalization.CultureInfo;
using StringComparison = System.StringComparison;

static class StringInput
{
  internal const char Space = ' ';
  internal const char SingleQuote = '\'';
  internal const char DoubleQuote = '"';
  internal const string StringSingleQuote = "'";
  internal const string StringDoubleQuote = "\"";
  internal const string EscapedSingleQuote = "''";
  internal const string EscapedDoubleQuote = "\\\"";
  internal const string Wildcard = "*";

  static internal CultureInfo CurrentCulture => field ??= CultureInfo.CurrentCulture;

  static internal CultureInfo InvariantCulture => field ??= CultureInfo.InvariantCulture;

  static internal string EscapeSingleQuoted(string text) => IsUnsafe(text)
    ? string.Concat(
      SingleQuote,
      System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(
        text
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

  static internal string Unescape(string escapedText) => UnescapeDoubleQuoted(
    UnescapeSingleQuoted(
      escapedText
    )
  );

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
