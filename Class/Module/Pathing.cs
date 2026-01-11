namespace Module
{
  using System;
  using System.IO;
  using System.Diagnostics;

  public static class Pathing
  {
    public static string Resolve(
      string location,
      string subpath = ""
    ) => Path.GetFullPath(
      subpath == string.Empty
        ? subpath
        : Path.GetRelativePath(
            location,
            subpath
          ),
      location
    );
  }
}
