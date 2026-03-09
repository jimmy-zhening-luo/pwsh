namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Reset,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("gr")]
sealed public partial class GitReset() : GitCommand("reset")
{
  private const string FlagHard = "--hard";

  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$",
    System.Text.RegularExpressions.RegexOptions.IgnoreCase
  )]
  static private partial System.Text.RegularExpressions.Regex GitTreeRegex();

  [Parameter(
    Position = 60,
    HelpMessage = "Tree spec to which to revert given as '[HEAD]([~]|^)[n]', defaulting to HEAD. If only the number index is given, branching defaults to '~'. If only branching is given, the index defaults to 0 (HEAD)."
  )]
  [ValidateNotNullOrWhiteSpace]
  [PathSpecCompletions]
  public string Tree
  {
    private get => tree;
    set
    {
      if (tree is not "")
      {
        var treeMatch = GitTreeRegex().Match(tree);

        if (
          treeMatch.Success
          && treeMatch.Groups["Step"].Value is var step
          && (
            step is ""
            || int.TryParse(
              step,
              out var _
            )
          )
        )
        {
          var branching = treeMatch.Groups["Branching"].Value is not ""
          and var b
            ? b
            : "~";

          tree = $"HEAD{branching}{step}";
        }
        else
        {
          Arguments.Insert(default, tree);

          tree = string.Empty;
        }
      }
    }
  }
  private string tree = string.Empty;

  [Parameter(
    HelpMessage = "Perform a non-destructive reset, equivalent to [--hard=false]"
  )]
  public SwitchParameter Soft { private get; set; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      if (Tree is "")
      {
        var treeMatch = GitTreeRegex().Match(WorkingDirectory);

        if (
          treeMatch.Success
          && treeMatch.Groups["Step"].Value is var step
          && (
            step is ""
            || int.TryParse(
              step,
              out var _
            )
          )
        )
        {
          var branching = treeMatch.Groups["Branching"].Value is not "" and var b
            ? b
            : "~";

          tree = $"HEAD{branching}{step}";
        }
        else
        {
          Arguments.Insert(default, WorkingDirectory);
        }
      }
      else
      {
        Arguments.Insert(default, WorkingDirectory);
      }

      WorkingDirectory = string.Empty;
    }

    if (Tree is not "")
    {
      Arguments.Insert(default, Tree);
    }

    _ = NativeArguments.RemoveAll(a => a is FlagHard);

    if (!Soft)
    {
      NativeArguments.Insert(default, FlagHard);

      AddCommand("Add-GitRepository");

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
}
