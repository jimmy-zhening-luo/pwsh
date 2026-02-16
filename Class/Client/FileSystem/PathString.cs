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
      : IO.Path.TrimEndingDirectorySeparator(
          normalPath
        );
  }
}
