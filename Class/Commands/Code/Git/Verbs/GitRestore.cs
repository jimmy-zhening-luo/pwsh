namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Restore,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("grp")]
public sealed class GitRestore() : GitCommand("pull")
{
  private protected sealed override void PreprocessArguments()
  {
    List<string> resetArguments = [.. ArgumentList];

    ArgumentList = [];

    if (WorkingDirectory is not "" && ResolveWorkingDirectory(Pwd()) is not "" && ResolveWorkingDirectory(WorkingDirectory) is "")
    {
      resetArguments.Insert(default, WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    string[] resetArgumentsArray = [.. resetArguments];

    AddCommand("Reset-GitRepository")
      .AddParameter("WorkingDirectory", WorkingDirectory)
      .AddParameter("ArgumentList", resetArgumentsArray);

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      Clear();
      Throw("Git returned error when resetting repository.");
    }
    else
    {
      Clear();
    }
  }

  private protected sealed override List<string> ParseArguments() => [];
}
