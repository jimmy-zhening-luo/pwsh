namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Push,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-push"
)]
[Alias("gs")]
public sealed class GitPush() : GitCommand("push")
{
  private protected sealed override void PreprocessArguments()
  {
    List<string> pusharguments = [];

    if (WorkingDirectory is not ""
     && ResolveWorkingDirectory(Pwd()) is not ""
     && ResolveWorkingDirectory(WorkingDirectory) is "")
    {
      ArgumentList = [WorkingDirectory, .. ArgumentList];

      WorkingDirectory = string.Empty;
    }

    AddCommand("Get-GitRepository")
      .AddParameter("WorkingDirectory", WorkingDirectory);

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      Clear();
      Throw("Git returned error when pulling repository prior to pushing");
    }
    else
    {
      Clear();
    }
  }

  private protected sealed override List<string> ParseArguments() => [];
}
