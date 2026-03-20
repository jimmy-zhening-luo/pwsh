namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Restore,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-reset"
)]
[Alias("grp")]
sealed public class GitRestore() : Git("pull")
{
  sealed override private protected void FinishSetup()
  {
    ClearArguments();

    _ = AddCommand(
      $@"{nameof(PowerModule)}\Reset-GitRepository"
    );

    _ = AddBoundParameters();

    BeginSteppablePipeline();
    ProcessSteppablePipeline();
    EndSteppablePipeline();

    CheckNativeError(
      "git error when resetting repository.",
      true
    );
  }
}
