namespace Module.Input.Path;

internal static partial class Normalizer
{
  internal static string Normalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = TrimRelativePrefix(
      DuplicateSeparatorRegex().Replace(
        ExpandEnvironmentVariables(
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
      : TrimEndingDirectorySeparator(
          normalPath
        );
  }
}
