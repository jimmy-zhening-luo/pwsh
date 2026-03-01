namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommunications.Write,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-commit"
)]
[Alias("gm")]
public sealed class GitCommit() : GitCommand("commit")
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
    List<string> arguments = [];
    List<string> messageWords = [];

    foreach (string word in ArgumentList)
    {
      if (word.Trim() is not "" and var w)
      {
        if (GitArgument.Regex().IsMatch(w))
        {
          arguments.Add(w);
        }
        else
        {
          messageWords.Add(w);
        }
      }
    }

    if (WorkingDirectory is not "" && ResolveWorkingDirectory(Pwd()) is not "" && ResolveWorkingDirectory(WorkingDirectory) is "")
    {
      if (GitArgument.Regex().IsMatch(WorkingDirectory))
      {
        arguments.Insert(default, WorkingDirectory);
      }
      else
      {
        messageWords.Insert(default, WorkingDirectory);
      }

      WorkingDirectory = string.Empty;
    }

    if (Message is not "")
    {
      if (GitArgument.Regex().IsMatch(Message))
      {
        arguments.Insert(default, Message);
      }
      else
      {
        messageWords.Insert(default, Message);
      }
    }

    if (allowEmpty && !arguments.Contains(FlagAllowEmpty))
    {
      arguments.Add(FlagAllowEmpty);
    }


    if (messageWords is [])
    {
      if (arguments.Contains(FlagAllowEmpty))
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

    arguments.InsertRange(
      default,
      [
        "-m",
        string.Join(' ', messageWords)
      ]
    );

    ArgumentList = [.. arguments];

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

  private protected sealed override List<string> ParseArguments() => [];
}
