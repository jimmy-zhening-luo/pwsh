namespace Module.Client.FileSystem;

internal static partial class PathString
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();

  private static string TrimRelativePrefix(
    string path
  ) => path == "."
    ? string.Empty
    : path.StartsWith(
        @".\"
      )
      ? path[2..]
      : path;
}
