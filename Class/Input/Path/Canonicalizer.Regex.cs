namespace Module.Input.Path;

internal static partial class Canonicalizer
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  internal static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
