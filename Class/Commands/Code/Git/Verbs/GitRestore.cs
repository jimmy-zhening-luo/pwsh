namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsData.Restore,
  "GitRepository",
  HelpUri = "https://git-scm.com/docs/git-reset"
)]
[Alias("grp")]
sealed public class GitRestore() : GitCommand("pull")
{
  sealed override private protected void PreprocessOtherArguments()
  {
    if (
      WorkingDirectory is not ""
      && ResolveWorkingDirectory(Pwd()) is not ""
      && ResolveWorkingDirectory(WorkingDirectory) is ""
    )
    {
      Arguments.AddFirst(WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    _ = AddCommand(@"PowerModule\Reset-GitRepository")
      .AddParameter(
        "ArgumentList",
        new Collection<string>(Arguments)
      );

    Arguments.Clear();

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
      "git error when resetting repository.",
      true
    );
  }
}
