namespace Module.Tab.Path;

public sealed partial class PathCompleter
{
  private static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = default
  )
  {
    string normalPath = Client.FileSystem.PathString.Normalize(
      path,
      preserveTrailingSeparator
    );

    string homedNormalPath = normalPath.StartsWith(
      '~'
    )
      ? normalPath.Length is 1
        ? Client.Environment.Known.Folder.Home()
        : normalPath[1] is '\\'
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

  private static string Denormalize(
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

  private static string CompletionString(
    string path,
    string accumulatedSubpath,
    bool trailingSeparator = default
  ) => Denormalize(
    System.IO.Path.GetFileName(
      path
    ),
    accumulatedSubpath,
    trailingSeparator
      ? @"\"
      : string.Empty
  );
}
