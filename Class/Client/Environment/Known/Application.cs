namespace Module.Client.Environment.Known;

internal static class Application
{
  internal static string VSCode => Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
  private static string? vscode;

  internal static string WinGet => Folder.LocalAppData(
    @"Microsoft\WindowsApps\winget.exe"
  );
  private static string? winget;

  internal static string Chrome => Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  private static string? chrome;

  internal static string Git => Folder.ProgramFiles(
    @"Git\cmd\git.exe"
  );
  private static string? git;

  internal static string Node => Folder.SystemDrive(
    @"nvm4w\nodejs\node.exe"
  );
  private static string? node;

  internal static string Npm => Folder.SystemDrive(
    @"nvm4w\nodejs\npm.ps1"
  );
  private static string? npm;

  internal static string Npx => Folder.SystemDrive(
    @"nvm4w\nodejs\npx.ps1"
  );
  private static string? npx;
}
