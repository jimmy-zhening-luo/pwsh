namespace Module;

internal static class Application
{
  internal static string VSCode
  {
    get => LocalAppData(
      @"Programs\Microsoft VS Code\bin\code.cmd"
    );
  }
}
