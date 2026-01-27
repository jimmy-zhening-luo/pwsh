namespace Module.Input.Path;

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
      : PSLocation(
          homedNormalPath
        );
  }
}
