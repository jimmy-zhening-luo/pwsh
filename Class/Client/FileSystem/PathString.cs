namespace Module.Client.FileSystem;

internal static partial class PathString
{
  internal static string Normalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = TrimRelativePrefix(
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
    );

    return preserveTrailingSeparator
      ? normalPath
      : System.IO.Path.TrimEndingDirectorySeparator(
          normalPath
        );
  }

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
