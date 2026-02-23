namespace Module.Client.Environment.Known;

internal static partial class Folder
{
  internal static string SystemDrive(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    FileSystem.PathString.Normalize(
      subpath
    ),
    Windows(
      ".."
    )
  );

  internal static string Code(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    FileSystem.PathString.Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
