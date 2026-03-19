namespace PowerModule.Client.Environment;

static class Application
{
  static internal string Chrome => field ??= Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );

  static internal string VSCode => field ??= Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );
}
