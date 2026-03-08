namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-push"
)]
[Alias("gs")]
sealed public class GitPush() : GitCommand("push")
{
  sealed private protected override void PreprocessOtherArguments()
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
