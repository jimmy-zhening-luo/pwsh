namespace Module.Commands.Code.Git;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = "https://git-scm.com/docs"
)]
[Alias("g")]
public sealed class GitInvoke : NativeCommand
{
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

  [Parameter(
    Position = default,
    HelpMessage = "Git command"
  )]
  [GitVerbCompletions]
  public string Verb { get; set; } = string.Empty;

  [Parameter(
    Position = 1,
    HelpMessage = "Repository path. For all verbs except 'clone', 'config', and 'init', the command will throw an error if there is no Git repository at the path."
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => version;
    set => version = value;
  }
  private bool version;

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> arguments = [];
    bool newable = default;

    if (Verb is not "")
    {
      if (Verbs.Contains(Verb.ToLower()))
      {
        if (System.Enum.TryParse<NewableVerb>(Verb, true, out var newableVerb))
        {
          newable = true;

          Verb = newableVerb.ToString();

          if (GitArgument.Regex().IsMatch(WorkingDirectory))
          {
            arguments.Add(WorkingDirectory);

            WorkingDirectory = string.Empty;
          }
        }
        else
        {
          if (
            WorkingDirectory is not ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              Pwd()
            ) is ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              WorkingDirectory
            ) is ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              Verb
            ) is not ""
          )
          {
            arguments.Add(WorkingDirectory);

            WorkingDirectory = Verb;
            Verb = "status";
          }
        }

        Verb = Verb.ToLower();
      }
    }
    else
    {
      if (version && ArgumentList is [])
      {
        newable = true;
      }
      else
      {
        Verb = "status";
      }
    }

    var repository = GitWorkingDirectory.Resolve(
      Pwd(),
      WorkingDirectory,
      newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        arguments.Insert(default, WorkingDirectory);

        repository = GitWorkingDirectory.Resolve(
          Pwd(),
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
      repository
    ];

    if (Verb is not "")
    {
      command.Add(Verb);
    }

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
    if (version)
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
