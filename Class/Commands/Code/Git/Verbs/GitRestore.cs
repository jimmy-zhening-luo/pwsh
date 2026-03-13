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
    if (DeferredVerbArgument is not null)
    {
      _ = Arguments.AddFirst(DeferredVerbArgument);
      DeferredVerbArgument = default;
    }

    var arguments = new string[Arguments.Count];
    Arguments.CopyTo(arguments, default);
    Arguments.Clear();

    _ = AddCommand(@"PowerModule\Reset-GitRepository")
      .AddParameter(
        "ArgumentList",
        arguments
      );

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
