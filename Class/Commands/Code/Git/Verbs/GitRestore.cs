namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Restore,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("grp")]
public sealed class GitRestore() : GitCommand("pull")
{
  private protected sealed override void PreprocessOtherArguments()
  {
    List<string> resetArguments = [.. Arguments];

    Arguments.Clear();

    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      resetArguments.Insert(default, WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    string[] resetArgumentsArray = [.. resetArguments];

    AddCommand("Reset-GitRepository")
      .AddParameter(
        "ArgumentList",
        resetArgumentsArray
      );

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
      "git error when resetting repository.",
      true
    );
  }
}
