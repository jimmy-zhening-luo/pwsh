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

  abstract private protected string WorkingDirectoryArtifactSubpath
  { get; }

  sealed override private protected Localizer? Location => location;
  Localizer? location;

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

  abstract private protected void CanonicalizeVerb();

  virtual private protected void FinishSetup()
  { }

  virtual private protected string[] GetVerbBaseArguments() => [];

  sealed override private protected void Setup()
  {
    CanonicalizeVerb();

    switch (WorkingDirectory)
    {
      case "":
        break;

      case var arg when IsNativeArgument(arg):
        _ = NativeArguments.AddFirst(arg);

        WorkingDirectory = string.Empty;

        break;

      case var path when System.IO.Path.Exists(
        System.IO.Path.Combine(
          Pwd(path),
          WorkingDirectoryArtifactSubpath
        )
      ):
        break;

      case var path when System.IO.Path.Exists(
        System.IO.Path.Combine(
          Client.Environment.Folder.Code(path),
          WorkingDirectoryArtifactSubpath
        )
      ):
        location = Client.Environment.Folder.Code;

        break;

      default:
        (
          DeferredVerbArgument,
          WorkingDirectory
        ) = (
          WorkingDirectory,
          string.Empty
        );

        break;
    }

    FinishSetup();
  }

  sealed override private protected string[] GetCommandRuntimeArguments()
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

  sealed override private protected string[] GetVerbRuntimeArguments() => DeferredVerbArgument is null
    ? GetVerbBaseArguments()
    : [
        DeferredVerbArgument,
        .. GetVerbBaseArguments(),
      ];

  sealed override private protected void ClearArguments()
  {
    DeferredVerbArgument = default;

    base.ClearArguments();
  }
}
