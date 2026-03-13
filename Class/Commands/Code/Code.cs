namespace PowerModule.Commands.Code;

abstract public class CodeNativeCommand(
  string CommandPath,
  IEnumerable<string> CommandBaseArguments,
  string? WorkingDirectoryParameterName,
  string? WorkingDirectoryPrefix,
  string? IntrinsicVerb
) : NativeCommand(
  CommandPath,
  IntrinsicVerb
)
{
  private protected string? DeferredVerbArgument;

  sealed override private protected Localizer? Location => location;
  private protected Localizer? WorkingDirectoryLocation
  { set; get; }

  sealed override private protected IEnumerable<string> CommandArguments => WorkingDirectory is ""
  || Pwd(WorkingDirectory) == Pwd()
    ? CommandBaseArguments
    : [
        .. CommandBaseArguments,
        .. ResolveWorkingDirectoryArguments(),
      ];

  sealed override private protected IEnumerable<string> VerbArguments => DeferredVerbArgument is null
    ? ParseArguments()
    : [
        DeferredVerbArgument,
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
      _ = NativeArguments.AddFirst(WorkingDirectory);

      WorkingDirectory = string.Empty;
    }

    PreprocessOtherArguments();
    PreprocessIntrinsicVerb();
    PreprocessWorkingDirectory();
  }

  private protected IEnumerable<string> ResolveWorkingDirectoryArguments()
  {
    var workingDirectoryArgument = string.Concat(
      WorkingDirectoryPrefix ?? string.Empty,
      Pwd(WorkingDirectory)
    );

    return WorkingDirectoryParameterName is null
      ? [workingDirectoryArgument]
      : [
          WorkingDirectoryParameterName,
          workingDirectoryArgument,
        ];
  }
}
