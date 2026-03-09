namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "ChildGitRepository",
  HelpUri = "https://git-scm.com/docs/git-pull"
)]
[Alias("gpp")]
sealed public class GitPullChild : CoreCommand
{
  sealed override private protected void Postprocess()
  {
    List<string> repositories = [];

    foreach (
      var directory in System.IO.Directory.GetDirectories(
        Client.Environment.Known.Folder.Code()
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

    var total = repositories.Count;
    int progress = default;

    WriteProgress(
      total,
      progress,
      "Pull"
    );

    string[] baseCommand = [
      "&",
      Client.Console.String.EscapeDoubleQuoted(
        Client.Environment.Known.Application.Git
      ),
      "-c",
      "color.ui=always",
      "-C",
    ];

    foreach (var repository in repositories)
    {
      AddScript(
        string.Join(
          Client.Console.String.Space,
          [
            .. baseCommand,
            Client.Console.String.EscapeDoubleQuoted(repository),
            "pull",
          ]
        )
      );

      BeginSteppablePipeline();
      ProcessSteppablePipeline();
      EndSteppablePipeline();
      ClearCommands();

      CheckNativeError(
        $"git error when pulling repository {repository}"
      );

      ++progress;

      WriteProgress(
        total,
        progress,
        "Pull"
      );
    }

    var suffix = total is 1
      ? string.Empty
      : "ies";

    WriteInformation(
      $"Pulled {progress} of {total} repositor{suffix}."
    );
  }
}
