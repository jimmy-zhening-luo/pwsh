namespace Module.Completer.Path;

internal static class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = Client.FileSystem.Path.Normalizer.Normalize(
      path,
      preserveTrailingSeparator
    );

    string homedNormalPath = normalPath.StartsWith(
      '~'
    )
      ? normalPath.Length == 1
        ? Client.Environment.Known.Folder.Home()
        : normalPath[1] == '\\'
          ? Client.Environment.Known.Folder.Home(
              normalPath[2..]
            )
          : normalPath
        : normalPath;

    return IO.Path.IsPathFullyQualified(
      homedNormalPath
    )
      ? homedNormalPath
      : PowerShellHost.CurrentDirectory(
          homedNormalPath
        );
  }

  internal static string Denormalize(
    string path,
    string location = "",
    string subpath = ""
  ) => IO
    .Path
    .Join(
      location,
      path,
      subpath
    )
    .Replace(
      '\\',
      '/'
    );

}
