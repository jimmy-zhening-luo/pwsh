namespace Module.Tab;

public partial class TabCompleter
{
  private protected static string Escape(
    string text
  ) => text.Contains(
    ' '
  )
    ? string.Concat(
        "'",
        System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(
          text
        ),
        "'"
      )
    : text;

  private protected static string Unescape(
    string escapedText
  ) => (
    escapedText.Length > 1
    && escapedText.StartsWith(
      '\''
    )
    && escapedText.EndsWith(
      '\''
    )
  )
    ? escapedText[1..^1].Replace(
        "''",
        "'"
      )
    : escapedText;
}
