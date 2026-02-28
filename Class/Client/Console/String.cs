namespace Module.Client.Console;

internal static partial class String
{
  internal static string EscapeSingleQuoted(string text) => text.Contains(' ')
    ? string.Concat(
        "'",
        System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(
          text
        ),
        "'"
      )
    : text;

  internal static string UnescapeSingleQuoted(string escapedText) => (
    escapedText.Length > 1
    && escapedText.StartsWith('\'')
    && escapedText.EndsWith('\'')
  )
    ? escapedText[1..^1].Replace(
        "''",
        "'"
      )
    : escapedText;

  internal static string EscapeDoubleQuoted(string text) => text.Contains(' ')
  ? string.Concat(
      "\"",
      text.Replace(
        "\"",
        "\\\""
      ),
      "\""
    )
  : text;
}
