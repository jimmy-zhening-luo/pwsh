namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommunications.Write,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-commit"
)]
[Alias("gm")]
public sealed partial class GitCommit() : GitCommand("commit")
{
  private static string FlagAllowEmpty => "--allow-empty";

  [Parameter(
    Position = 60,
    HelpMessage = "Commit message, which must be non-empty except on an empty commit on which it defaults to 'No message'"
  )]
  [PathSpecCompletions]
  public string Message
  {
    get => message;
    set => message = value.Trim();
  }
  private string message = string.Empty;

  [Parameter(
    HelpMessage = "Only commit files that are already staged"
  )]
  public SwitchParameter Staged
  {
    get => staged;
    set => staged = value;
  }
  private bool staged;

  [Parameter(
    HelpMessage = "Allow an empty commit, equivalent to git commit --allow-empty"
  )]
  public SwitchParameter AllowEmpty
  {
    get => allowEmpty;
    set => allowEmpty = value;
  }
  private bool allowEmpty;

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
      if (NativeArgumentRegex().IsMatch(WorkingDirectory))
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
      if (NativeArgumentRegex().IsMatch(Message))
      {
        NativeArguments.Insert(default, Message);
      }
      else
      {
        messageWords.Insert(default, Message);
      }
    }

    if (allowEmpty && !NativeArguments.Contains(FlagAllowEmpty))
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
        Throw(
          "Commit message is required unless --allow-empty is specified"
        );
      }
    }

    Message = string.Join(Client.Console.String.Space, messageWords);

    AddCommand("Add-GitRepository")
      .AddParameter("WorkingDirectory", WorkingDirectory);

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      Clear();
      Throw("Git returned error when staging files for commit");
    }
    else
    {
      Clear();
    }
  }

  private protected sealed override List<string> ParseArguments() => Message is ""
  ? []
  : [
      "-m",
      Message
    ];
}
