namespace Module.Client.FileSystem.Path;

internal static partial class Normalizer
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();

  private static string TrimRelativePrefix(
    string path
  ) => path.StartsWith(
    '.'
  )
    && (
      path.Length == 1
      || path[1] == '\\'
    )
    ? path.Length == 1
      ? string.Empty
      : path[2..]
    : path;
}
