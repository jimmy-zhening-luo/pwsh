namespace Module.Input.PathResolution;

using Regex = System.Text.RegularExpressions.Regex;
using GeneratedRegex = System.Text.RegularExpressions.GeneratedRegexAttribute;

public static partial class Canonicalizer
{
  [GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  public static partial Regex DuplicateSeparatorRegex();
}
