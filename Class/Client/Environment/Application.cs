namespace PowerModule.Client.Environment;

static class Application
{
  static internal string Chrome => chrome ??= Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
  static string? chrome;

  static internal string VSCode => vscode ??= Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
  static string? vscode;
}
