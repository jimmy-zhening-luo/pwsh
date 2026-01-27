namespace Module.Input.Path;

internal static partial class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false
  ) => AnchorHome(
    Normalize(
      path,
      preserveTrailingSeparator
    )
  );

  private static string Normalize(
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

  internal static string Denormalize(
    string path,
    string location = "",
    string subpath = ""
  ) => Join(
    location,
    path,
    subpath
  )
    .Replace(
      '\\',
      '/'
    );

  private static string RemoveRelativeRoot(
    string path
  ) => IsRelativelyRooted(
    path
  )
    ? path.Length == 1
      ? string.Empty
      : path[2..]
    : path;

  private static string AnchorHome(
    string path
  ) => IsHomeRooted(
    path
  )
    ? Home(
        path[1..]
      )
    : path;

  private static bool IsRelativelyRooted(
    string path
  ) => path.StartsWith(
    '.'
  )
    && (
      path.Length == 1
      || path[1] == '\\'
    );

  private static bool IsHomeRooted(
    string path
  ) => path.StartsWith(
    '~'
  )
    && (
        path.Length == 1
      || path[1] == '\\'
    );
}
