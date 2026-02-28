namespace Module.Commands.Code.Node;

public abstract class NodeCommand(
  string IntrinsicVerb = ""
) : NativeCommand
{
  private protected static readonly HashSet<string> Verbs = [
    "access",
    "adduser",
    "audit",
    "bugs",
    "cache",
    "ci",
    "completion",
    "config",
    "dedupe",
    "deprecate",
    "diff",
    "dist-tag",
    "docs",
    "doctor",
    "edit",
    "exec",
    "explain",
    "explore",
    "find-dupes",
    "fund",
    "help",
    "help-search",
    "init",
    "install",
    "install-ci-test",
    "install-test",
    "link",
    "login",
    "logout",
    "ls",
    "org",
    "outdated",
    "owner",
    "pack",
    "ping",
    "pkg",
    "prefix",
    "profile",
    "prune",
    "publish",
    "query",
    "rebuild",
    "repo",
    "restart",
    "root",
    "run",
    "sbom",
    "search",
    "shrinkwrap",
    "star",
    "stars",
    "start",
    "stop",
    "team",
    "test",
    "token",
    "undeprecate",
    "uninstall",
    "unpublish",
    "unstar",
    "update",
    "version",
    "view",
    "whoami",
  ];

  private protected static readonly Dictionary<string, string> Aliases = new()
  {
    ["issues"] = "bugs",
    ["c"] = "config",
    ["ddp"] = "dedupe",
    ["home"] = "docs",
    ["why"] = "explain",
    ["create"] = "init",
    ["add"] = "install",
    ["i"] = "install",
    ["in"] = "install",
    ["ln"] = "link",
    ["cit"] = "install-ci-test",
    ["it"] = "install-test",
    ["list"] = "ls",
    ["author"] = "owner",
    ["rb"] = "rebuild",
    ["find"] = "search",
    ["s"] = "search",
    ["se"] = "search",
    ["t"] = "test",
    ["unlink"] = "uninstall",
    ["remove"] = "uninstall",
    ["rm"] = "uninstall",
    ["r"] = "uninstall",
    ["un"] = "uninstall",
    ["up"] = "update",
    ["upgrade"] = "update",
    ["info"] = "view",
    ["show"] = "view",
    ["v"] = "view",
  };

  public string IntrinsicVerb { get; private protected set; } = IntrinsicVerb;

  [Parameter(
    Position = 50,
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> command = [
      "&",
      "npm.ps1",
      "--color=always",
    ];

    List<string> arguments = [
      .. ParseArguments(),
    ];

    if (IntrinsicVerb is "")
    {
      Throw("No npm command given.");
    }
    else if (
      Aliases.TryGetValue(
        IntrinsicVerb.ToLower(),
        out var alias
      )
    )
    {
      IntrinsicVerb = alias;
    }
    else if (
      Verbs.TryGetValue(
        IntrinsicVerb.ToLower(),
        out var verb
      )
    )
    {
      IntrinsicVerb = verb;
    }
    else
    {
      Throw($"Unknown npm command: {IntrinsicVerb}");
    }

    if (WorkingDirectory is not "")
    {
      if (NodeWorkingDirectory.Test(Pwd(), WorkingDirectory))
      {
        var packagePrefix = WorkingDirectory is ""
          ? ""
          : "--prefix="
            + Client.File.PathString.FullPathLocationRelative(
                Pwd(),
                WorkingDirectory
              );

        if (packagePrefix is not "")
        {
          command.Add(packagePrefix);
        }
      }
      else
      {
        arguments.Insert(default, WorkingDirectory);

      }
    }

    WorkingDirectory = string.Empty;

    command.Add(IntrinsicVerb.ToLower());

    if (d)
    {
      arguments.Add("-D");
      d = false;
    }
    if (e)
    {
      arguments.Add("-E");
      e = false;
    }
    if (p)
    {
      arguments.Add("-P");
      p = false;
    }

    command.AddRange(arguments);

    return command;
  }
}
