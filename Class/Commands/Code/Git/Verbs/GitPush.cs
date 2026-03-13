namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  "GitRepository",
  HelpUri = $"{GitHelpLink}/git-push"
)]
[Alias("gs")]
sealed public class GitPush() : Git("push")
{
  sealed override private protected void PreprocessOtherArguments()
  {
    if (DeferredVerbArgument is not "")
    {
      _ = Arguments.AddFirst(DeferredVerbArgument);
      DeferredVerbArgument = string.Empty;
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
