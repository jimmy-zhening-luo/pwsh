namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommunications.Write,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-commit"
)]
[Alias("gm")]
public sealed partial class GitCommit() : GitCommand("commit")
{
  private const string FlagAllowEmpty = "--allow-empty";

  [Parameter(
    Position = 60,
    HelpMessage = "Commit message, defaulting to 'No message' on an empty commit"
  )]
  [PathSpecCompletions]
  public string Message
  {
    get => message;
    set => message = value;
  }
  private string message = string.Empty;

  [Parameter(
    HelpMessage = "Only commit files that are already staged"
  )]
  public SwitchParameter Staged { get; set; }

  [Parameter(
    HelpMessage = "Allow commit with no changes, equivalent to --allow-empty"
  )]
  public SwitchParameter AllowEmpty { get; set; }

  private protected sealed override void PreprocessArguments()
  {
    List<string> messageWords = [.. ArgumentList];
    ArgumentList = [];

    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      if (IsNativeArgument(WorkingDirectory))
      {
        NativeArguments.Insert(default, WorkingDirectory);
      }
      else
      {
        messageWords.Insert(default, WorkingDirectory);
      }

      WorkingDirectory = string.Empty;
    }

    if (Message is not "")
    {
      if (IsNativeArgument(Message))
      {
        NativeArguments.Insert(default, Message);
      }
      else
      {
        messageWords.Insert(default, Message);
      }
    }

    if (AllowEmpty && !NativeArguments.Contains(FlagAllowEmpty))
    {
      NativeArguments.Add(FlagAllowEmpty);
    }

    if (messageWords is [])
    {
      if (NativeArguments.Contains(FlagAllowEmpty))
      {
        messageWords.Add("No message");
      }
      else
      {
        ThrowError(
          "Commit message is required unless --allow-empty is specified"
        );
      }
    }

    Message = string.Join(Client.Console.String.Space, messageWords);

    if (!Staged)
    {
      AddCommand("Add-GitRepository")
        .AddParameter("WorkingDirectory", WorkingDirectory);

      ProcessSteppablePipeline();
      EndSteppablePipeline();

      if (HadNativeError)
      {
        Clear();
        ThrowError("Git returned error when staging files for commit");
      }
      else
      {
        Clear();
      }
    }
  }

  private protected sealed override List<string> ParseArguments() => Message is ""
  ? []
  : [
      "-m",
      Message
    ];
}
