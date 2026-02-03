namespace Module.FileSystem.Path;

internal static partial class Normalizer
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
