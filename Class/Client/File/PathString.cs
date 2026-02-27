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
    var normalPath = TrimRelativePrefix(
      ExpandHomePrefix(
        DuplicateSeparatorRegex().Replace(
          System
            .Environment
            .ExpandEnvironmentVariables(
              path
                ?.Trim()
                ?? string.Empty
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

  private static string ExpandHomePrefix(string path) => path switch
  {
    "~" => Environment.Known.Folder.Home(),
    _ when path.StartsWith(@"~\") => Environment.Known.Folder.Home(
        path[2..]
      ),
    _ => path
  };

  private static string TrimRelativePrefix(string path) => path switch
  {
    "." => string.Empty,
    _ when path.StartsWith(@".\") => path[2..],
    _ => path
  };

  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
