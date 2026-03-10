namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "ChildGitRepository",
  HelpUri = "https://git-scm.com/docs/git-pull"
)]
[Alias("gpp")]
sealed public class GitPullChild : CoreCommand
{
  static IEnumerable<string> EnumerateRepository()
  {
    foreach (
      var directory in System.IO.Directory.EnumerateDirectories(
        Client.Environment.Folder.Code()
      )
    )
    {
      if (
        System.IO.Directory.Exists(
          System.IO.Path.Combine(
            directory,
            ".git"
          )
        )
      )
      {
        yield return directory;
      }
    }

    yield break;
  }

  sealed override private protected void Postprocess()
  {
    ushort progress = default;

    var baseCommand = $"& {Client.String.EscapeDoubleQuoted(
      Client.Environment.Application.Git
    )} -c color.ui=always -C";

    foreach (var repository in EnumerateRepository())
    {
      _ = AddScript(
        $"{baseCommand} {Client.String.EscapeDoubleQuoted(
          repository
        )} pull"
      );

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();
      ClearCommands();

      CheckNativeError(
        $"git error when pulling repository {repository}"
      );

      ++progress;
    }

    var suffix = progress is 1
      ? string.Empty
      : "ies";

    WriteInformation(
      $"Pulled {progress} repositor{suffix}."
    );
  }
}
