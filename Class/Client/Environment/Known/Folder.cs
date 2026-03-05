namespace Module.Client.Environment.Known;

using SpecialFolder = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows() => Local.GetFolder(SpecialFolder.Windows);
  internal static string Windows(string path) => Local.GetFolder(
    SpecialFolder.Windows,
    path
  );

  internal static string ProgramFiles() => Local.GetFolder(SpecialFolder.ProgramFiles);
  internal static string ProgramFiles(string path) => Local.GetFolder(
    SpecialFolder.ProgramFiles,
    path
  );

  internal static string AppData() => Local.GetFolder(SpecialFolder.ApplicationData);
  internal static string AppData(string path) => Local.GetFolder(
    SpecialFolder.ApplicationData,
    path
  );

  internal static string LocalAppData() => Local.GetFolder(SpecialFolder.LocalApplicationData);
  internal static string LocalAppData(string path) => Local.GetFolder(
    SpecialFolder.LocalApplicationData,
    path
  );

  internal static string Home() => Local.GetFolder(SpecialFolder.UserProfile);
  internal static string Home(string path) => Local.GetFolder(
    SpecialFolder.UserProfile,
    path
  );
}
