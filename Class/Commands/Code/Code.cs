namespace PowerModule.Commands.Code;

abstract public class NativeCodeCommand(
  string CommandPath,
  string? IntrinsicVerb,
  string[] CommandBaseArguments,
  string? WorkingDirectoryParameterName = default,
  string? WorkingDirectoryPrefix = default
) : NativeCommand(
  CommandPath,
  IntrinsicVerb,
  CommandBaseArguments
)
{
  private protected string? DeferredVerbArgument;

  sealed override private protected Localizer? Location => WorkingDirectoryLocation;
  private protected Localizer? WorkingDirectoryLocation
  { set; get; }

  sealed override private protected string[] CommandRuntimeArguments => ResolveWorkingDirectoryArguments();

  sealed override private protected string[] VerbArguments => DeferredVerbArgument is null
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

  virtual private protected string[] ParseArguments() => [];

  sealed override private protected void ClearArguments()
  {
    DeferredVerbArgument = default;

    base.ClearArguments();
  }

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

    PreprocessIntrinsicVerb();
    PreprocessWorkingDirectory();
    PreprocessOtherArguments();
  }

  string[] ResolveWorkingDirectoryArguments()
  {
    if (ReanchorPath(WorkingDirectory) == Pwd())
    {
      return [];
    }

    var workingDirectoryArgument = string.Concat(
      WorkingDirectoryPrefix ?? string.Empty,
      ReanchorPath(WorkingDirectory)
    );

    return WorkingDirectoryParameterName is null
      ? [workingDirectoryArgument]
      : [
          WorkingDirectoryParameterName,
          workingDirectoryArgument,
        ];
  }
}
