namespace Module.Input.Path;

internal static partial class Normalizer
{
  internal static string Normalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = AnchorHome(
      TrimRelativePrefix(
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
