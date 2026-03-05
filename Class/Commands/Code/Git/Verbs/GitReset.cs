namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Reset,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("gr")]
public sealed partial class GitReset() : GitCommand("reset")
{
  private const string FlagHard = "--hard";

  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$",
    System.Text.RegularExpressions.RegexOptions.IgnoreCase
  )]
  private static partial System.Text.RegularExpressions.Regex GitTreeRegex();

  [Parameter(
    Position = 60,
    HelpMessage = "Tree spec to which to revert given as '[HEAD]([~]|^)[n]', defaulting to HEAD. If only the number index is given, branching defaults to '~'. If only branching is given, the index defaults to 0 (HEAD)."
  )]
  [PathSpecCompletions]
  public string Tree
  {
    get => tree;
    set
    {
      if (tree is not "")
      {
        var treeMatch = GitTreeRegex().Match(tree);

        if (treeMatch.Success && treeMatch.Groups["Step"].Value is var step
        && (step is ""
          || int.TryParse(step, out var _)
        ))
        {
          var branching = treeMatch.Groups["Branching"].Value is not "" and var b ? b : "~";

          tree = $"HEAD{branching}{step}";
        }
        else
        {
          ArgumentList = [tree, .. ArgumentList];
          tree = string.Empty;
        }
      }
    }
  }
  private string tree = string.Empty;

  [Parameter(
    HelpMessage = "Perform a non-destructive reset, equivalent to [--hard=false]"
  )]
  public SwitchParameter Soft { get; set; }

  private protected sealed override void PreprocessArguments()
  {
    List<string> resetArguments = [.. ArgumentList];

    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      if (Tree is not "")
      {
        resetArguments.Insert(default, WorkingDirectory);
      }
      else
      {
        var treeMatch = GitTreeRegex().Match(WorkingDirectory);

        if (treeMatch.Success && treeMatch.Groups["Step"].Value is var step && (step is "" || int.TryParse(step, out var _)))
        {
          var branching = treeMatch.Groups["Branching"].Value is not "" and var b ? b : "~";

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

    ArgumentList = [.. resetArguments];

    _ = NativeArguments.RemoveAll(a => a is FlagHard);

    if (!Soft)
    {
      NativeArguments.Insert(default, FlagHard);

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
}
