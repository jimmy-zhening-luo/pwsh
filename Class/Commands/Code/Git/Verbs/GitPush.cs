namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-push"
)]
[Alias("gs")]
public sealed class GitPush() : GitCommand("push")
{
  private protected sealed override void PreprocessOtherArguments()
  {
    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      Arguments.Insert(default, WorkingDirectory);

      WorkingDirectory = string.Empty;
    }

    AddCommand("Get-GitRepository");

    if (WorkingDirectory is not "")
    {
      AddParameter(
        "WorkingDirectory",
        WorkingDirectory
      );
    }

    ProcessSteppablePipeline();
    EndSteppablePipeline();
    ClearCommands();

    CheckNativeError(
      "git error when pulling repository prior to pushing",
      true
    );
  }
}
