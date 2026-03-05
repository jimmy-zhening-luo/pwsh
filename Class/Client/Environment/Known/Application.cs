namespace Module.Client.Environment.Known;

internal static class Application
{
  internal static System.Lazy<string> VSCode = new(
    () => Folder.LocalAppData(
      @"Programs\Microsoft VS Code\bin\code.cmd"
    )
  );

  internal static string WinGet => winget ??= Folder.LocalAppData(
    @"Microsoft\WindowsApps\winget.exe"
  );
  private static string? winget;

  internal static string Chrome => chrome ??= Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  private static string? chrome;

  internal static string Git => git ??= Folder.ProgramFiles(
    @"Git\cmd\git.exe"
  );
  private static string? git;

  internal static string Node => node ??= Folder.SystemDrive(
    @"nvm4w\nodejs\node.exe"
  );
  private static string? node;

  internal static string Npm => npm ??= Folder.SystemDrive(
    @"nvm4w\nodejs\npm.ps1"
  );
  private static string? npm;

  internal static string Npx => npx ??= Folder.SystemDrive(
    @"nvm4w\nodejs\npx.ps1"
  );
  private static string? npx;
}
