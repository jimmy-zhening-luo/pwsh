namespace Module.Client.Environment.Known;

internal static class Application
{
  internal static string VSCode => Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );

  internal static string WinGet => Folder.LocalAppData(
    @"Microsoft\WindowsApps\winget.exe"
  );

  internal static string Chrome => Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );

  internal static string Git => Folder.ProgramFiles(
    @"Git\cmd\git.exe"
  );

  internal static string Node => Folder.SystemDrive(
    @"nvm4w\nodejs\node.exe"
  );

  internal static string Npm => Folder.SystemDrive(
    @"nvm4w\nodejs\npm.ps1"
  );

  internal static string Npx => Folder.SystemDrive(
    @"nvm4w\nodejs\npx.ps1"
  );
}
