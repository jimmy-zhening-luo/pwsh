namespace Module.Client.Environment.Known;

using EnvironmentFolder = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows() => Local.GetFolder(EnvironmentFolder.Windows);
  internal static string Windows(string path) => Local.GetFolder(
    EnvironmentFolder.Windows,
    path
  );

  internal static string ProgramFiles() => Local.GetFolder(EnvironmentFolder.ProgramFiles);
  internal static string ProgramFiles(string path) => Local.GetFolder(
    EnvironmentFolder.ProgramFiles,
    path
  );

  internal static string AppData() => Local.GetFolder(EnvironmentFolder.ApplicationData);
  internal static string AppData(string path) => Local.GetFolder(
    EnvironmentFolder.ApplicationData,
    path
  );

  internal static string LocalAppData() => Local.GetFolder(EnvironmentFolder.LocalApplicationData);
  internal static string LocalAppData(string path) => Local.GetFolder(
    EnvironmentFolder.LocalApplicationData,
    path
  );

  internal static string Home() => Local.GetFolder(EnvironmentFolder.UserProfile);
  internal static string Home(string path) => Local.GetFolder(
    EnvironmentFolder.UserProfile,
    path
  );
}
