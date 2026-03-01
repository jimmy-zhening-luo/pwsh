namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string IntrinsicVerb = "",
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh)
{
  internal sealed class WorkingDirectoryCompletionsAttribute : PathCompletionsAttribute
  {
    internal WorkingDirectoryCompletionsAttribute() : base(
      @"~\code",
      PathItemType.Directory
    )
    {
      Flat = true;
    }
  }

  internal sealed class PathSpecCompletionsAttribute() : PathCompletionsAttribute(
    "",
    PathItemType.File
  );

  [Parameter(
    Position = 50,
    HelpMessage = "Working directory path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory
  {
    get => workingDirectory;
    set => workingDirectory = value.Trim();
  }
  private string workingDirectory = string.Empty;
}
