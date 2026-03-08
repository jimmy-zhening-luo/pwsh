namespace Module.Commands.Code;

abstract public class RemoteNativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh)
{
  sealed internal class PathSpecCompletionsAttribute() : Tab.PathCompletionsAttribute(
    ItemType: Tab.PathItemType.File
  );

  private protected readonly List<string> DeferredVerbArguments = [];

  abstract private protected string[] CommandBaseArguments { get; }

  abstract private protected string[] WorkingDirectoryArguments { get; }

  sealed override private protected string[] CommandArguments => [
    .. CommandBaseArguments,
    .. WorkingDirectoryArguments,
  ];

  sealed override private protected string[] VerbArguments => [
    .. DeferredVerbArguments,
    .. ParseArguments(),
  ];

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

  abstract private protected void PreprocessIntrinsicVerb();

  abstract private protected void PreprocessWorkingDirectory();

  virtual private protected void PreprocessOtherArguments()
  { }

  virtual private protected List<string> ParseArguments() => [];

  sealed override private protected void PreprocessArguments()
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
    PreprocessIntrinsicVerb();
    PreprocessWorkingDirectory();
  }
}
