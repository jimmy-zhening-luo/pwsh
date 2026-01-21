namespace Module.Input;

public static class Escaper
{
  public static string Escape(
    string text
  ) => text.Contains(' ')
    ? "'"
      + System.Management.Automation.Language.CodeGeneration.EscapeSingleQuotedStringContent(text)
      + "'"
    : text;

  public static string Unescape(
    string escapedText
  ) => (
    escapedText.Length > 1
    && escapedText.StartsWith('\'')
    && escapedText.EndsWith('\'')
  )
    ? escapedText[1..^1].Replace(
        "''",
        "'"
      )
    : escapedText;
}
