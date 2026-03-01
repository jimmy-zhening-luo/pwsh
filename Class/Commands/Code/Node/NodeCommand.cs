namespace Module.Commands.Code.Node;

public abstract class NodeCommand(
  string IntrinsicVerb = ""
) : NativeCommand
{
  private protected sealed class NodeVerbCompletionsAttribute() : CompletionsAttribute([.. Verbs]);

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

  private protected string IntrinsicVerb
  {
    get => intrinsicVerb;
    set => intrinsicVerb = value.Trim();
  }
  private string intrinsicVerb = IntrinsicVerb.Trim();

  [Parameter(
    Position = 50,
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory
  {
    get => workingDirectory;
    set => workingDirectory = value.Trim();
  }
  private string workingDirectory = string.Empty;

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

    switch (WorkingDirectory)
    {
      case "":
        break;

      case string path when !IsNodePackage(path):
        arguments.Insert(default, path);
        break;

      case string path when Pwd(path) is string fullPath
        && fullPath != Pwd():
        command.Add(
          $"--prefix={Pwd(fullPath)}"
        );
        break;
    }

    WorkingDirectory = string.Empty;

    switch (IntrinsicVerb)
    {
      case "":
        break;

      case string verb when Aliases.TryGetValue(
        verb.ToLower(),
        out var alias
      ):
        command.Add(alias);
        break;

      case string verb when Verbs.TryGetValue(
        verb.ToLower(),
        out var exactVerb
      ):
        command.Add(exactVerb);
        break;

      default:
        arguments.Insert(default, IntrinsicVerb);
        break;
    }

    IntrinsicVerb = string.Empty;

    if (d)
    {
      d = false;
      arguments.Add("-D");
    }
    if (e)
    {
      e = false;
      arguments.Add("-E");
    }
    if (p)
    {
      p = false;
      arguments.Add("-P");
    }

    command.AddRange(arguments);

    return command;
  }

  private protected bool IsNodePackage(string path) => System.IO.File.Exists(
    System.IO.Path.Combine(
      Pwd(path),
      "package.json"
    )
  );

}
