namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string IntrinsicVerb = ""
) : NativeCommand
{
  internal enum NewableVerb
  {
    clone,
    config,
    init
  }

  internal static readonly HashSet<string> Verbs = [
    "switch",
    "merge",
    "diff",
    "stash",
    "tag",
    "config",
    "remote",
    "submodule",
    "fetch",
    "checkout",
    "branch",
    "rm",
    "mv",
    "ls-files",
    "ls-tree",
    "init",
    "status",
    "clone",
    "pull",
    "add",
    "commit",
    "push",
    "reset",
  ];

  private protected string IntrinsicVerb { get; set; } = IntrinsicVerb;

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

    var newable = System.Enum.TryParse<NewableVerb>(
      IntrinsicVerb,
      true,
      out var _
    )
      ? true
      : false;

    var repository = ResolveWorkingDirectory(
      WorkingDirectory,
      newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        arguments.Insert(default, WorkingDirectory);

        repository = ResolveWorkingDirectory(
          Pwd(),
          newable
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
      IntrinsicVerb,
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

  private protected string ResolveWorkingDirectory(
    string path,
    bool newable = default
  ) => System.IO.Directory.Exists(
    System.IO.Path.Combine(
      path,
      newable ? string.Empty : ".git"
    )
  )
    ? Pwd(path)
    : System.IO.Directory.Exists(
        System.IO.Path.Combine(
          Client.Environment.Known.Folder.Code(path),
          newable ? string.Empty : ".git"
        )
      )
      ? Client.Environment.Known.Folder.Code(path)
      : string.Empty;
}
