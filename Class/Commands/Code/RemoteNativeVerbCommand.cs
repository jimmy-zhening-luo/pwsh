namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string IntrinsicVerb = "",
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh: SkipSsh)
{
  internal sealed class PathSpecCompletionsAttribute() : Tab.Path.PathCompletionsAttribute(
    ItemType: Tab.Path.PathItemType.File
  );

  [Parameter(
    Position = 50,
    HelpMessage = "Working directory path"
  )]
  [Tab.Path.PathCompletions(
    @"~\code",
    Tab.Path.PathItemType.Directory,
    Flat: true
  )]
  public string WorkingDirectory
  {
    get => workingDirectory;
    set => workingDirectory = value;
  }
  private string workingDirectory = string.Empty;

  private protected readonly List<string> DeferredVerbArguments = [];

  private protected virtual List<string> ParseArguments() => [];

  private protected sealed override List<string> NativeCommandVerbArguments() => [
    .. DeferredVerbArguments,
    .. ParseArguments(),
  ];
}
