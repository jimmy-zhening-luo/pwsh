namespace Completer.PathCompleter
{
  public static partial class Canonicalizer
  {
    public static string Normalize(
      string path,
      bool preserveTrailingSeparator = false
    )
    {
      string normalPath = RemoveRelativeRoot(
        DuplicateSeparatorRegex().Replace(
          System
            .Environment
            .ExpandEnvironmentVariables(path)
            .Replace('/', '\\'),
          @"\"
        )
      );

      return preserveTrailingSeparator
        ? normalPath
        : System.IO.Path.TrimEndingDirectorySeparator(
            normalPath
          );
    }

    public static string Denormalize(
      string path,
      string location = "",
      string subpath = ""
    ) => System
      .IO
      .Path
      .Join(
        location,
        path,
        subpath
      )
      .Replace('\\', '/');

    public static bool IsRelativelyRooted(string path) => path.StartsWith('.')
      && (
        path.Length == 1
        || path[1] == '\\'
      );

    public static bool IsHomeRooted(string path) => path.StartsWith('~')
      && (
        path.Length == 1
        || path[1] == '\\'
      );

    public static string RemoveRelativeRoot(string path) => IsRelativelyRooted(path)
      ? path.Length == 1
        ? string.Empty
        : path[2..]
      : path;

    public static string Home() => System.Environment.GetFolderPath(
      System
        .Environment
        .SpecialFolder
        .UserProfile
    );

    public static string AnchorHome(string path) => IsHomeRooted(path)
      ? Home() + path[1..]
      : path;
  }
}
