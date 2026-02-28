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

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V
  {
    get => v;
    set => v = value;
  }
  private bool v;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> arguments = [];

    arguments.AddRange(ParseArguments());

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
      Verb
    ];

    if (d)
    {
      arguments.Add("-d");
    }
    if (e)
    {
      arguments.Add("-E");
    }
    if (i)
    {
      arguments.Add("-i");
    }
    if (o)
    {
      arguments.Add("-o");
    }
    if (p)
    {
      arguments.Add("-P");
    }
    if (v)
    {
      arguments.Add("-v");
    }

    if (arguments is not [])
    {
      command.AddRange(arguments);
    }

    return command;
  }
}
