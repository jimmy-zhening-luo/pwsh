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

    return IsPathFullyQualified(
      normalPath
    )
      ? normalPath
      : PSLocation(
          normalPath
        );
  }
}
