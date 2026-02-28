namespace Module.Commands.Code.Git.Verbs;

[Cmdlet(
  VerbsCommon.Get,
  "ChildGitRepository",
  HelpUri = "https://git-scm.com/docs/git-pull"
)]
[Alias("gpp")]
public sealed class GitPullChild : CoreCommand
{
  private static ProgressRecord GetProgress(
    int total,
    int progress
  ) => new(
    0,
    "Pull",
    $"{progress}/{total}"
  )
  {
    PercentComplete = 100 * progress / total,
    RecordType = progress == total
      ? ProgressRecordType.Completed
      : ProgressRecordType.Processing,
  };

  private protected sealed override void Postprocess()
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
          System.IO.Path.Combine(directory, ".git")
        )
      )
      {
        repositories.Add(directory);
      }
    }

    var total = repositories.Count;
    int progress = default;

    WriteProgress(
      GetProgress(
        total,
        progress
      )
    );

    List<string> baseCommand = [
      "&",
      "\"" + Client.Environment.Known.Application.Git + "\"",
      "-c",
      "color.ui=always",
      "-C",
    ];

    foreach (var repository in repositories)
    {
      List<string> command = [
        .. baseCommand,
        Client.Console.String.EscapeDoubleQuoted(repository),
        "pull"
      ];

      AddScript(string.Join(' ', command));

      ProcessSteppablePipeline();
      EndSteppablePipeline();

      if (HadNativeErrors)
      {
        WriteWarning("git error");
      }

      Clear();

      ++progress;

      WriteProgress(
        GetProgress(
          total,
          progress
        )
      );
    }

    var suffix = total is 1 ? "" : "ies";

    WriteObject(
      $"Pulled {progress} of {total} repositor{suffix}."
    );
  }
}
