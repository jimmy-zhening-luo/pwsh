namespace Module.Client.File;

internal static partial class PathString
{
  internal const char Here = '.';
  internal const char Home = '~';
  internal const char Separator = '\\';
  internal const char AltSeparator = '/';
  internal const string SeparatorString = @"\";

  internal static string FullPathLocationRelative(
    string location,
    string path,
    bool preserveTrailingSeparator = default
  ) => System.IO.Path.GetFullPath(
    Normalize(
      path,
      preserveTrailingSeparator
    ),
    location
  );

  internal static string Normalize(
    string path,
    bool preserveTrailingSeparator = default
  )
  {
    var normalPath = TrimRelativePrefix(
      ExpandHomePrefix(
        DeduplicateSeparator(
          System.Environment.ExpandEnvironmentVariables(path)
            .Replace(
              AltSeparator,
              Separator
            )
        )
      )
    );

    return preserveTrailingSeparator
      ? normalPath
      : System.IO.Path.TrimEndingDirectorySeparator(normalPath);
  }

  private static string ExpandHomePrefix(string path) => path switch
  {
    [Home] => Environment.Known.Folder.Home(),
    [
      Home,
      Separator,
      .. var subpath,
    ] => Environment.Known.Folder.Home(subpath),
    _ => path,
  };

  private static string TrimRelativePrefix(string path) => path switch
  {
    [Here] => string.Empty,
    [
      Here,
      Separator,
      .. var subpath,
    ] => subpath,
    _ => path,
  };

  private static string DeduplicateSeparator(string path) => DuplicateSeparatorRegex().Replace(
    path,
    SeparatorString
  );
  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\{2,})"
  )]
  private static partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();
}
