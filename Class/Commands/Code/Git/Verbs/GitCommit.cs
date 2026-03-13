namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommunications.Write,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-commit"
)]
[Alias("gm")]
sealed public class GitCommit() : Git("commit")
{
  const string FlagAllowEmpty = "--allow-empty";

  [Parameter(
    Position = 60,
    HelpMessage = "Commit message, defaulting to 'No message' on an empty commit"
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Message
  { private get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Allow commit with no changes, equivalent to --allow-empty"
  )]
  public SwitchParameter AllowEmpty
  { private get; set; }

  [Parameter(
    HelpMessage = "Only commit files that are already staged"
  )]
  public SwitchParameter Staged
  { private get; init; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (Message is not "")
    {
      AddFirst(Message);
    }

    if (DeferredVerbArgument is not null)
    {
      _ = Arguments.AddFirst(DeferredVerbArgument);
      DeferredVerbArgument = default;
    }

    if (NativeArguments.Contains(FlagAllowEmpty))
    {
      AllowEmpty = true;
    }
    else if (AllowEmpty)
    {
      _ = NativeArguments.AddLast(FlagAllowEmpty);
    }

    if (AllowEmpty && Arguments.Count is 0)
    {
      _ = Arguments.AddLast("No message");
    }

    Message = string.Join(
      Client.StringInput.Space,
      Arguments
    );
    Arguments.Clear();

    System.ArgumentException.ThrowIfNullOrEmpty(
      Message,
      nameof(Message)
    );

    if (!Staged)
    {
      _ = AddCommand(
        @"PowerModule\Add-GitRepository"
      );

      if (WorkingDirectory is not "")
      {
        _ = AddParameter(
          "WorkingDirectory",
          WorkingDirectory
        );
      }

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();

      CheckNativeError(
        "git error when staging files for commit",
        true
      );
    }
  }

  sealed override private protected IEnumerable<string> ParseArguments() => [
    "-m",
    Message,
  ];
}
