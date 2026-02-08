namespace Module.Environment.Environmental;

using static System.Environment;

internal static partial class Environmental
{
  internal static string Env(
    string variable
  ) => GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  internal static string EnvPath(
    SpecialFolder folder,
    string subpath = ""
  ) => IO.Path.GetFullPath(
    FileSystem.Path.Normalizer.Normalize(
      subpath
    ),
    GetFolderPath(
      folder
    )
  );
}
