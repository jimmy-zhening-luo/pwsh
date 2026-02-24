namespace Module.Tab.Path;

internal static class Canonicalizer
{
  internal static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = false
  )
  {
    string normalPath = Client.FileSystem.PathString.Normalize(
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

    return System.IO.Path.IsPathFullyQualified(
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
  ) => System.IO
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
