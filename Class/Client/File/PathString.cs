namespace Module.Client.File;

internal static partial class PathString
{
  internal const char PathSeparator = '\\';
  internal const string PathSeparatorString = @"\";

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
              PathSeparator
            ),
          PathSeparatorString
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
    [
      '~',
      PathSeparator,
      .. var subpath,
    ] => Environment.Known.Folder.Home(subpath),
    _ => path,
  };

  private static string TrimRelativePrefix(string path) => path switch
  {
    "." => string.Empty,
    [
      '.',
      PathSeparator,
      .. var subpath,
    ] => subpath,
    _ => path,
  };

  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\\\+)"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
