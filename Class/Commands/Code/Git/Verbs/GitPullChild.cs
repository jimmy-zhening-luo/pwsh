namespace PowerModule.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "ChildGitRepository"
)]
[Alias("gpp")]
sealed public class GitPullChild : CoreCommand
{
  static List<string> ListRepositories()
  {
    List<string> repositories = [];

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
        repositories.Add(directory);
      }
    }

    return repositories;
  }

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

    var suffix = progress is 1
      ? string.Empty
      : "ies";

    WriteInformation(
      $"Pulled {progress} repositor{suffix}."
    );
  }
}
