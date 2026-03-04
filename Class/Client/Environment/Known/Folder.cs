namespace Module.Client.Environment.Known;

internal static partial class Folder
{
  internal static string Windows(string? path = default) => Local.GetFolder(
    System.Environment.SpecialFolder.Windows,
    path
  );

  internal static string ProgramFiles(string? path = default) => Local.GetFolder(
    System.Environment.SpecialFolder.ProgramFiles,
    path
  );

  internal static string AppData(string? path = default) => Local.GetFolder(
    System.Environment.SpecialFolder.ApplicationData,
    path
  );

  internal static string LocalAppData(string? path = default) => Local.GetFolder(
    System.Environment.SpecialFolder.LocalApplicationData,
    path
  );

  internal static string Home(string? path = default) => Local.GetFolder(
    System.Environment.SpecialFolder.UserProfile,
    path
  );
}
