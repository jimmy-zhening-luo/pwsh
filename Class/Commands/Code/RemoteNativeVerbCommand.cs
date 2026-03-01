namespace Module.Commands.Code;

public abstract class RemoteNativeVerbCommand(
  string IntrinsicVerb = "",
  bool SkipSsh = default
) : NativeVerbCommand(IntrinsicVerb, SkipSsh)
{
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
