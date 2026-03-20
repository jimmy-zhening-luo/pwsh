namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  GitNoun,
  HelpUri = $"{GitHelpLink}/git-push"
)]
[Alias("gs")]
sealed public class GitPush() : Git("push")
{
  sealed override private protected void FinishSetup()
  {
    if (DeferredVerbArgument is not null)
    {
      _ = Arguments.AddFirst(DeferredVerbArgument);
      DeferredVerbArgument = default;
    }

    _ = AddCommand(
      $@"{nameof(PowerModule)}\Get-GitRepository"
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
      "git error when pulling repository prior to pushing",
      true
    );
  }
}
