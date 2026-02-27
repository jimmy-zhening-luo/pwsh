namespace Module.Client.File;

internal static partial class PathString
{
  internal static string FullPathLocationRelative(
    string location,
    string? path,
    bool preserveTrailingSeparator = default
  ) => System.IO.Path.GetFullPath(
    Normalize(
      path,
      preserveTrailingSeparator
    ),
    location
  );

  internal static string Normalize(
    string? path,
    bool preserveTrailingSeparator = default
  )
  {
    if (path is null)
    {
      return string.Empty;
    }

    var normalPath = TrimRelativePrefix(
      ExpandHomePrefix(
        DuplicateSeparatorRegex().Replace(
          System
            .Environment
            .ExpandEnvironmentVariables(
              path.Trim()
            )
            .Replace(
              '/',
              '\\'
            ),
          @"\"
        )
      )
    );

    return preserveTrailingSeparator
      ? normalPath
      : System.IO.Path.TrimEndingDirectorySeparator(normalPath);
  }

  private static string ExpandHomePrefix(string path) => path.StartsWith('~')
    ? path.Length is 1
      ? Environment.Known.Folder.Home()
      : path[1] is '\\'
        ? Environment.Known.Folder.Home(
            path[2..]
          )
        : path
      : path;

  private static string TrimRelativePrefix(string path) => path is "."
    ? string.Empty
    : path.StartsWith(@".\")
      ? path[2..]
      : path;

  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
