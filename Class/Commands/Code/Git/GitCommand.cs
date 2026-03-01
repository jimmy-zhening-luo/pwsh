namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string IntrinsicVerb = ""
) : NativeCommand
{
  private protected sealed class GitVerbCompletionsAttribute() : CompletionsAttribute([.. Verbs]);

  private enum NewableVerb
  {
    clone,
    config,
    init
  }

  private static readonly HashSet<string> Verbs = [
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

  private protected string IntrinsicVerb
  {
    get => intrinsicVerb;
    set => intrinsicVerb = value.Trim();
  }
  private string intrinsicVerb = IntrinsicVerb.Trim();

  [Parameter(
    Position = 50,
    HelpMessage = "Repository path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory
  {
    get => workingDirectory;
    set => workingDirectory = value.Trim();
  }
  private string workingDirectory = string.Empty;

  private protected sealed override string CommandPath => Client.Environment.Known.Application.Git;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> arguments = [
      .. ParseArguments(),
    ];

    bool newable = default;
    switch (IntrinsicVerb)
    {
      case "" when v:
        newable = true;
        break;

      case "":
        IntrinsicVerb = "status";
        break;

      case string verb when System.Enum.TryParse<NewableVerb>(
        verb,
        true,
        out var newableVerb
      ):
        newable = true;
        IntrinsicVerb = newableVerb.ToString();

        break;

      case string verb when Verbs.TryGetValue(
        verb.ToLower(),
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;
        break;
    }

    var pwd = Pwd();
    var repository = ResolveWorkingDirectory(
      WorkingDirectory,
      newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        arguments.Insert(default, WorkingDirectory);

        repository = ResolveWorkingDirectory(pwd, newable);
      }
    }

    if (repository is "")
    {
      Throw(
         newable
          ? $"Path does not support the current git operation: {WorkingDirectory}"
          : $"Path is not a git repository: {WorkingDirectory}"
      );
    }

    List<string> command = [
      "-c",
      "color.ui=always",
    ];

    if (
      repository is not ""
      && repository != pwd
    )
    {
      command.Add("-C");
      command.Add(repository);
    }

    if (IntrinsicVerb is not "")
    {
      command.Add(IntrinsicVerb);
    }

    command.AddRange(arguments);

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
