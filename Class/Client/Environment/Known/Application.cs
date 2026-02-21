namespace Module.Client.Environment.Known;

internal static class Application
{
  internal static string VSCode => Folder.LocalAppData(
    @"Programs\Microsoft VS Code\bin\code.cmd"
  );

  internal static string Chrome => Folder.ProgramFiles(
    @"Google\Chrome\Application\chrome.exe"
  );
}
