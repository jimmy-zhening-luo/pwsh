namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Reset,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-reset"
)]
[Alias("gr")]
sealed public partial class GitReset() : Git("reset")
{
  const string FlagHard = "--hard";

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
  [Tab.PathCompletions(ItemType: Tab.PathItemType.File)]
  public string Tree
  {
    private get => tree;
    set
    {
      if (
        GitTreeRegex().Match(
          tree
        ) is var treeMatch
        && treeMatch.Success
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
        _ = Arguments.AddFirst(tree);

        tree = string.Empty;
      }
    }
  }
  string tree = string.Empty;

  [Parameter(
    HelpMessage = "Perform a non-destructive reset, equivalent to [--hard=false]"
  )]
  public SwitchParameter Soft
  { private get; init; }

  sealed override private protected void PreprocessOtherArguments()
  {
    if (DeferredVerbArgument is not null)
    {
      if (Tree is "")
      {
        var treeMatch = GitTreeRegex().Match(
          DeferredVerbArgument
        );

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
          _ = Arguments.AddFirst(DeferredVerbArgument);
        }
      }
      else
      {
        _ = Arguments.AddFirst(DeferredVerbArgument);
      }

      DeferredVerbArgument = default;
    }

    if (Tree is not "")
    {
      _ = Arguments.AddFirst(Tree);
    }

    if (!Soft)
    {
      _ = NativeArguments.AddFirst(FlagHard);

      _ = AddCommand(@"PowerModule\Add-GitRepository");

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
      ClearCommands();

      CheckNativeError(
        "git error when staging files for commit",
        true
      );
    }
  }
}
