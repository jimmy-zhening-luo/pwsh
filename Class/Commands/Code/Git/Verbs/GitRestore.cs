namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Restore,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-reset"
)]
[Alias("grp")]
sealed public class GitRestore() : Git("pull")
{
  sealed override private protected void PreprocessOtherArguments()
  {
    ClearArguments();

    _ = AddCommand(
      @"PowerModule\Reset-GitRepository"
    );

    AddBoundParameters();

    BeginSteppablePipeline();
    ProcessSteppablePipeline();
    EndSteppablePipeline();

    CheckNativeError(
      "git error when resetting repository.",
      true
    );
  }
}
