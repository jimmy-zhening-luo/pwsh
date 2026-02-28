namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string Verb,
  bool Newable = default
) : NativeCommand
{
  [Parameter(
    Position = default,
    HelpMessage = "Repository path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> command = [
      "&",
      Client.Environment.Known.Application.Git,
      "-c",
      "color.ui=always",
      "-C",
      repository,
      Verb
    ];

    List<string> arguments = [
      .. ParseArguments(),
    ];

    var repository = GitWorkingDirectory.Resolve(
      Pwd(),
      WorkingDirectory,
      Newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        command.Add(WorkingDirectory);

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

    command.AddRange(arguments);

    if (e)
    {
      arguments.Add("-E");
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
