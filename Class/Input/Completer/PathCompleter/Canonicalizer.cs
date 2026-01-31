namespace Module.Input.Completer.PathCompleter;

internal static class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = Normalize(
      path,
      preserveTrailingSeparator
    );

    string homedNormalPath = normalPath.StartsWith(
      '~'
    )
      ? normalPath.Length == 1
        ? Home()
        : normalPath[1] == '\\'
          ? Home(
              normalPath[2..]
            )
          : normalPath
        : normalPath;

    return IsPathFullyQualified(
      homedNormalPath
    )
      ? homedNormalPath
      : PSConsole.CurrentDirectory(
          homedNormalPath
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

}
