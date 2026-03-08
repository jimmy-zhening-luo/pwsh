namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh: SkipSsh)
{
  internal sealed class PathSpecCompletionsAttribute() : Tab.PathCompletionsAttribute(
    ItemType: Tab.PathItemType.File
  );

  private protected abstract string[] WorkingDirectoryArguments { get; }

  [Parameter(
    Position = 50,
    ValueFromPipeline = true,
    HelpMessage = "Working directory path"
  )]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory,
    Flat: true
  )]
  public string WorkingDirectory { get; set; } = string.Empty;

  private protected readonly List<string> DeferredVerbArguments = [];

  private protected abstract List<string> NativeCommandBaseArguments();

  private protected virtual void PreprocessOtherArguments()
  { }

  private protected virtual List<string> ParseArguments() => [];

  private protected sealed override void PreprocessArguments()
  {
    if (
      WorkingDirectory is not ""
      && IsNativeArgument(WorkingDirectory)
    )
    {
      NativeArguments.Insert(default, WorkingDirectory);

      WorkingDirectory = string.Empty;
    }

    PreprocessOtherArguments();
  }

  private protected sealed override List<string> NativeCommandArguments() => [
    .. NativeCommandBaseArguments(),
    .. WorkingDirectoryArguments,
  ];

  private protected sealed override List<string> NativeCommandVerbArguments() => [
    .. DeferredVerbArguments,
    .. ParseArguments(),
  ];
}
