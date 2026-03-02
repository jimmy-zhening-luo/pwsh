namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string IntrinsicVerb = "",
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh)
{
  internal sealed class WorkingDirectoryCompletionsAttribute : Tab.Path.PathCompletionsAttribute
  {
    internal WorkingDirectoryCompletionsAttribute() : base(
      @"~\code",
      Tab.Path.PathItemType.Directory
    )
    {
      Flat = true;
    }
  }

  internal sealed class PathSpecCompletionsAttribute() : Tab.Path.PathCompletionsAttribute(
    "",
    Tab.Path.PathItemType.File
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

  private protected readonly List<string> DeferredVerbArguments = [];

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> NativeCommandVerbArguments() => [
    .. DeferredVerbArguments,
    .. ParseArguments(),
  ];
}
