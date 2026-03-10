namespace PowerModule.Client.Environment;

static internal class Application
{
  static internal string Chrome => chrome ??= Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  static private string? chrome;

  static internal string VSCode => vscode ??= Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
  static private string? vscode;

  static internal string WinGet => winget ??= Folder.LocalAppData(
    @"Microsoft\WindowsApps\winget.exe"
  );
  static private string? winget;

  static internal string Git => git ??= Folder.ProgramFiles(
    @"Git\cmd\git.exe"
  );
  static private string? git;

  static internal string Npm => npm ??= Folder.SystemDrive(
    @"nvm4w\nodejs\npm.ps1"
  );
  static private string? npm;
}
