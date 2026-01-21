namespace Module.Input.PathResolution;

public static partial class Canonicalizer
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  public static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
