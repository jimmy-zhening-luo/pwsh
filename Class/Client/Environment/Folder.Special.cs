namespace PowerModule.Client.Environment;

using EnvironmentFolder = System.Environment.SpecialFolder;

static internal partial class Folder
{
  static internal string Windows() => GetFolder(EnvironmentFolder.Windows);
  static internal string Windows(string path) => GetFolder(
    EnvironmentFolder.Windows,
    path
  );

  static internal string ProgramFiles() => GetFolder(EnvironmentFolder.ProgramFiles);
  static internal string ProgramFiles(string path) => GetFolder(
    EnvironmentFolder.ProgramFiles,
    path
  );

  static internal string AppData() => GetFolder(EnvironmentFolder.ApplicationData);
  static internal string AppData(string path) => GetFolder(
    EnvironmentFolder.ApplicationData,
    path
  );

  static internal string LocalAppData() => GetFolder(EnvironmentFolder.LocalApplicationData);
  static internal string LocalAppData(string path) => GetFolder(
    EnvironmentFolder.LocalApplicationData,
    path
  );

  static internal string Home() => GetFolder(EnvironmentFolder.UserProfile);
  static internal string Home(string path) => GetFolder(
    EnvironmentFolder.UserProfile,
    path
  );
}
