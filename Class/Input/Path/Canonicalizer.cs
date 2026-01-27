namespace Module.Input.Path;

internal static partial class Canonicalizer
{
  internal static string CanonicalizeRootedPath(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string canonicalPath = AnchorHome(
      CanonicalizeAbsolutePath(
        path,
        preserveTrailingSeparator
      )
    );

    return IsPathFullyQualified(
      canonicalPath
    )
      ? canonicalPath
      : PSLocation(
          canonicalPath
        );
  }

  internal static string CanonicalizeAbsolutePath(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = RemoveRelativeRoot(
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

  private static string RemoveRelativeRoot(
    string path
  ) => path.StartsWith(
    '.'
  )
    && (
      path.Length == 1
      || path[1] == '\\'
    )
    ? path.Length == 1
      ? string.Empty
      : path[2..]
    : path;

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
