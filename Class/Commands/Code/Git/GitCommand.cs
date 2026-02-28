namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string Verb,
  bool Newable = default
) : NativeCommand
{
  [Parameter(
    Position = 50,
    HelpMessage = "Repository path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> arguments = [
      .. ParseArguments(),
    ];

    var newable = System.Enum.TryParse<Git.NewableVerb>(
      Verb,
      true,
      out var _
    )
      ? true
      : false;

    var repository = GitWorkingDirectory.Resolve(
      Pwd(),
      WorkingDirectory,
      Newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        arguments.Insert(default, WorkingDirectory);

        repository = GitWorkingDirectory.Resolve(
          Pwd(),
          Pwd(),
          Newable
        );
      }

      if (repository is "")
      {
        Throw(
          $"Path {WorkingDirectory} is not a git repository."
        );
      }
    }

    List<string> command = [
      "&",
      Client.Environment.Known.Application.Git,
      "-c",
      "color.ui=always",
      "-C",
      repository,
      Verb,
      .. arguments,
    ];

    if (e)
    {
      command.Add("-E");
      e = false;
    }
    if (p)
    {
      command.Add("-P");
      p = false;
    }

    return command;
  }
}
