namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh: SkipSsh)
{
  sealed internal class PathSpecCompletionsAttribute() : Tab.PathCompletionsAttribute(
    ItemType: Tab.PathItemType.File
  );

  private protected abstract string[] WorkingDirectoryArguments { get; }

  [Parameter(
    Position = 50,
    HelpMessage = "Working directory path"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory,
    Flat: true
  )]
  public string WorkingDirectory
  {
    private protected get;
    set;
  } = string.Empty;

  private protected readonly List<string> DeferredVerbArguments = [];

  private protected abstract List<string> NativeCommandBaseArguments();

  private protected virtual void PreprocessOtherArguments()
  { }

  private protected virtual List<string> ParseArguments() => [];

  sealed private protected override void PreprocessArguments()
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

  sealed private protected override List<string> NativeCommandArguments() => [
    .. NativeCommandBaseArguments(),
    .. WorkingDirectoryArguments,
  ];

  sealed private protected override List<string> NativeCommandVerbArguments() => [
    .. DeferredVerbArguments,
    .. ParseArguments(),
  ];
}
