namespace Module.Tab.Path;

public partial class PathCompletionsAttribute
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
}
