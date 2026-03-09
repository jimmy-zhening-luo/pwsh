namespace PowerModule.Commands.Code;

abstract public class CodeNativeCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeCommand(IntrinsicVerb, SkipSsh)
{
  private protected readonly List<string> DeferredVerbArguments = [];

  abstract private protected IEnumerable<string> CommandBaseArguments
  { get; }

  abstract private protected IEnumerable<string> WorkingDirectoryArguments
  { get; }

  sealed override private protected IEnumerable<string> CommandArguments => [
    .. CommandBaseArguments,
    .. WorkingDirectoryArguments,
  ];

  sealed override private protected IEnumerable<string> VerbArguments => [
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
    Flat = true
  )]
  public string WorkingDirectory
  { private protected get; set; } = string.Empty;

  abstract private protected void PreprocessIntrinsicVerb();

  abstract private protected void PreprocessWorkingDirectory();

  virtual private protected void PreprocessOtherArguments()
  { }

  virtual private protected IEnumerable<string> ParseArguments() => [];

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

    PreprocessIntrinsicVerb();
    PreprocessWorkingDirectory();
    PreprocessOtherArguments();
  }
}
