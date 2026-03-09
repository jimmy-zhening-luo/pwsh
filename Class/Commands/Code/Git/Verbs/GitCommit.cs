namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommunications.Write,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-commit"
)]
[Alias("gm")]
sealed public class GitCommit() : GitCommand("commit")
{
  private const string FlagAllowEmpty = "--allow-empty";

  [Parameter(
    Position = 60,
    HelpMessage = "Commit message, defaulting to 'No message' on an empty commit"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Message { private get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Only commit files that are already staged"
  )]
  public SwitchParameter Staged { private get; set; }

  [Parameter(
    HelpMessage = "Allow commit with no changes, equivalent to --allow-empty"
  )]
  public SwitchParameter AllowEmpty { private get; set; }

  sealed override private protected void PreprocessOtherArguments()
  {
    List<string> messageWords = [.. Arguments];
    Arguments.Clear();

    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      messageWords.Insert(default, WorkingDirectory);

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

    if (
      messageWords is []
      && NativeArguments.Contains(FlagAllowEmpty)
    )
    {
      messageWords.Add("No message");
    }

    Message = string.Join(Client.String.Space, messageWords);

    System.ArgumentException.ThrowIfNullOrEmpty(
      Message,
      nameof(Message)
    );

    if (!Staged)
    {
      AddCommand(@"PowerModule\Add-GitRepository");

      if (WorkingDirectory is not "")
      {
        AddParameter(
          "WorkingDirectory",
          WorkingDirectory
        );
      }

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();
      ClearCommands();

      CheckNativeError(
        "git error when staging files for commit",
        true
      );
    }
  }

  sealed override private protected string[] ParseArguments() => Message is ""
  ? []
  : [
      "-m",
      Message
    ];
}
