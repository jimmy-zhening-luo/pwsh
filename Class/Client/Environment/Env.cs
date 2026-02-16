namespace Module.Client.Environment;

using static System.Environment;

internal static partial class Env
{
  internal static string Get(
    string variable
  ) => GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  internal static string GetFolder(
    SpecialFolder folder,
    string subpath = ""
  ) => IO.Path.GetFullPath(
    FileSystem.PathString.Normalize(
      subpath
    ),
    GetFolderPath(
      folder
    )
  );
}
