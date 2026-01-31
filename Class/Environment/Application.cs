namespace Module.Environment;

internal static class Application
{
  internal static string VSCode => vscode ??= LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
  private static string? vscode;

  internal static string Chrome => chrome ??= ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  private static string? chrome;
}
