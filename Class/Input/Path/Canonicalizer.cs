namespace Module.Input.Path;

internal static partial class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false,
    bool anchorRelative = false
  )
  {
    string normalPath = AnchorHome(
      RemoveRelativeRoot(
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
    && (
      path.Length == 1
      || path[1] == '\\'
    )
    ? Home(
        path[1..]
      )
    : path;
}
