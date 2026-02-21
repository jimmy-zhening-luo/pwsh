namespace Module.Client.Environment;

internal static partial class Env
{
  private static readonly Dictionary<System.Environment.SpecialFolder, string> = new ();

  internal static string Get(
    string variable
  ) => System.Environment.GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  internal static string GetFolder(
    System.Environment.SpecialFolder folder,
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    FileSystem.PathString.Normalize(
      subpath
    ),
    System.Environment.GetFolderPath(
      folder
    )
  );
}
