namespace Module.Commands.Code.Git;

public static class GitWorkingDirectory
{
  public static string Code => Client.Environment.Known.Folder.Code();

  public static string Resolve(
    string currentLocation,
    string path,
    bool newable = default
  ) => TestPath(
    currentLocation,
    path,
    newable
  )
    ? FullPath(currentLocation, path)
    : !System.IO.Path.IsPathRooted(path)
      && TestPath(
          Code,
          path,
          newable
        )
      ? FullPath(Code, path)
      : string.Empty;

  private static string FullPath(
    string location,
    string path
  ) => Client.File.PathString.FullPathLocationRelative(location, path);

  private static bool TestPath(
    string location,
    string path,
    bool newable
  ) => System.IO.Directory.Exists(
    newable
      ? FullPath(location, path)
      : System.IO.Path.Combine(
          FullPath(location, path),
          ".git"
        )
  );
}
