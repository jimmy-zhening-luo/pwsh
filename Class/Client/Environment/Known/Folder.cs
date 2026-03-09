namespace PowerModule.Client.Environment.Known;

using EnvironmentFolder = System.Environment.SpecialFolder;

static internal partial class Folder
{
  static internal string Windows() => Local.GetFolder(EnvironmentFolder.Windows);
  static internal string Windows(string path) => Local.GetFolder(
    EnvironmentFolder.Windows,
    path
  );

  static internal string ProgramFiles() => Local.GetFolder(EnvironmentFolder.ProgramFiles);
  static internal string ProgramFiles(string path) => Local.GetFolder(
    EnvironmentFolder.ProgramFiles,
    path
  );

  static internal string AppData() => Local.GetFolder(EnvironmentFolder.ApplicationData);
  static internal string AppData(string path) => Local.GetFolder(
    EnvironmentFolder.ApplicationData,
    path
  );

  static internal string LocalAppData() => Local.GetFolder(EnvironmentFolder.LocalApplicationData);
  static internal string LocalAppData(string path) => Local.GetFolder(
    EnvironmentFolder.LocalApplicationData,
    path
  );

  static internal string Home() => Local.GetFolder(EnvironmentFolder.UserProfile);
  static internal string Home(string path) => Local.GetFolder(
    EnvironmentFolder.UserProfile,
    path
  );
}
