namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Reset,
  GitNoun,
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
    private get;
    set
    {
      if (
        GitTreeRegex().Match(value)
        is var treeMatch
        && treeMatch.Success
        && treeMatch.Groups["Step"].Value
        is var step
        && (
          step is ""
          || int.TryParse(
            step,
            out _
          )
        )
      )
      {
        var branching = treeMatch.Groups["Branching"].Value
        is not ""
        and var b
          ? b
          : "~";

        field = $"HEAD{branching}{step}";
      }
      else
      {
        _ = Arguments.AddFirst(value);

        field = string.Empty;
      }
    }
  } = string.Empty;

  [Parameter(
    HelpMessage = $"Perform a non-destructive reset, equivalent to [{FlagHard}=false]"
  )]
  public SwitchParameter Soft
  { private get; init; }

  sealed override private protected void FinishSetup()
  {
    if (DeferredVerbArgument is not null)
    {
      if (Tree is "")
      {
        Tree = DeferredVerbArgument;
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

      _ = AddCommand(
        $@"{nameof(PowerModule)}\Add-GitRepository"
      );

      if (WorkingDirectory is not "")
      {
        _ = AddParameter(
          nameof(WorkingDirectory),
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
}
