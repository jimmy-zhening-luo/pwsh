namespace Module.PC.Environment.Known;

internal static class Application
{
  internal static string VSCode => vscode ??= Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
  private static string? vscode;

  internal static string Chrome => chrome ??= Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  private static string? chrome;
}
