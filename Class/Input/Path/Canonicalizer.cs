namespace Module.Input.Path;

internal static partial class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = AnchorHome(
      Normalize(
        path,
        preserveTrailingSeparator
      )
    );

    return IsPathFullyQualified(
      normalPath
    )
      ? normalPath
      : PSLocation(
          normalPath
        );
  }

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

  private static string AnchorHome(
    string path
  ) => path.StartsWith(
    '~'
  )
    ? path.Length == 1
      ? Home()
      : path[1] == '\\'
        ? Home(
            path[2..]
          )
        : path
      : path;
}
