namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "ChildGitRepository"
)]
[Alias("gpp")]
sealed public class GitPullChild : CoreCommand
{
  sealed override private protected void Postprocess()
  {
    ushort progress = default;

    var baseCommand = "git -c color.ui=always -C";

    foreach (var repository in ListRepositories())
    {
      ClearCommands();

      _ = AddScript(
        $"{baseCommand} {Client.StringInput.EscapeDoubleQuoted(
          repository
        )} pull"
      );

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();

      CheckNativeError(
        $"git error when pulling repository {repository}"
      );

      ++progress;
    }

    WriteInformation(
      $"Pulled {progress} repositories."
    );
  }
}
