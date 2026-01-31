namespace Module;

internal static class Application
{
  internal static string VSCode
  {
    get => vscode ??= LocalAppData(
      @"Programs\Microsoft VS Code\bin\code.cmd"
    );
  }
  private static string? vscode;
}
