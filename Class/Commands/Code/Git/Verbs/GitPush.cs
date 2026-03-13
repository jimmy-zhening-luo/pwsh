namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-push"
)]
[Alias("gs")]
sealed public class GitPush() : GitCommand("push")
{
  sealed override private protected void PreprocessOtherArguments()
  {
    if (HasThrowawayWorkingDirectory())
    {
      _ = Arguments.AddFirst(WorkingDirectory);

      WorkingDirectory = string.Empty;
    }

    _ = AddCommand(@"PowerModule\Get-GitRepository");

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
      "git error when pulling repository prior to pushing",
      true
    );
  }
}
