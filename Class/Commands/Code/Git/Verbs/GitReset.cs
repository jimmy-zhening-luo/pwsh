namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Reset,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("gr")]
public sealed class GitReset() : GitCommand("reset")
{
  private static string FlagAllowEmpty => "--allow-empty";

  [Parameter(
    Position = 60,
    HelpMessage = "The tree spec to which to revert given as '[HEAD]([~]|^)[n]'. Defaults to HEAD. If only the number index is given, defaults to '~' branching. If only branching is given, defaults to index 0 (HEAD)."
  )]
  [PathSpecCompletions]
  public string Tree
  {
    get => tree;
    set
    {
      var trimmed = value.Trim();

      if (trimmed is not "")
      {
        var treeMatch = GitArgument.TreeRegex().Match(trimmed);

        if (treeMatch.Success && treeMatch.Groups["step"].Value is var step
        && (step is ""
          || int.TryParse(step, out var _)
        ))
        {
          var branching = treeMatch.Groups["branching"].Value is not "" and var b ? b : "~";

          tree = $"HEAD{branching}{step}";
        }
        else
        {
          ArgumentList = [trimmed, .. ArgumentList];
          tree = string.Empty;
        }
      }
    }
  }
  private string tree = string.Empty;

  [Parameter(
    HelpMessage = "Non-destructive reset, equivalent to running git reset without --hard"
  )]
  public SwitchParameter Soft
  {
    get => soft;
    set => soft = value;
  }
  private bool soft;

  private protected sealed override void PreprocessArguments()
  {
    List<string> resetArguments = [.. ArgumentList];

    if (WorkingDirectory is not "" && ResolveWorkingDirectory(Pwd()) is not "" && ResolveWorkingDirectory(WorkingDirectory) is "")
    {
      if (Tree is not "")
      {
        resetArguments.Insert(default, WorkingDirectory);
      }
      else
      {
        var treeMatch = GitArgument.TreeRegex().Match(WorkingDirectory);

        if (treeMatch.Success && treeMatch.Groups["step"].Value is var step && (step is "" || int.TryParse(step, out var _)))
        {
          var branching = treeMatch.Groups["branching"].Value is not "" and var b ? b : "~";

          tree = $"HEAD{branching}{step}";
        }
        else
        {
          resetArguments.Insert(default, WorkingDirectory);
        }
      }

      WorkingDirectory = string.Empty;
    }

    if (Tree is not "")
    {
      resetArguments.Insert(default, Tree);
    }

    _ = resetArguments.RemoveAll(a => a is "--hard");

    if (!soft)
    {
      resetArguments.Insert(default, "--hard");

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

    ArgumentList = [.. resetArguments];
  }

  private protected sealed override List<string> ParseArguments() => [];
}
