namespace Module.Commands.Code.Node;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
public sealed class NpmInvoke : NativeCommand
{
  private static readonly HashSet<string> Verbs = [
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

  private static readonly Dictionary<string, string> Aliases = new()
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

  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [NodeVerbCompletions]
  public string Verb { get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
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
    List<string> command = [
      "&",
      "npm.ps1",
      "--color=always",
    ];

    List<string> arguments = [];

    if (ArgumentList is not [])
    {
      arguments.AddRange(ArgumentList);
    }

    ArgumentList = [];

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

        WorkingDirectory = string.Empty;
      }
    }

    if (
      Verb is not ""
      && Verb.StartsWith('-')
      || !Verbs.Contains(Verb.ToLower())
      && !Aliases.ContainsKey(Verb.ToLower())
    )
    {
      var deferredVerb = arguments is []
        ? ""
        : arguments.Find(
            argument => Verbs.Contains(
              argument.ToLower()
            )
          );

      if (deferredVerb is not (null or ""))
      {
        _ = arguments.Remove(deferredVerb);
      }

      arguments.Insert(default, Verb);

      Verb = deferredVerb ?? string.Empty;
    }

    if (Verb is not "")
    {
      command.Add(Verb.ToLower());

      if (d)
      {
        arguments.Add("-D");
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
    }
    else
    {
      if (version)
      {
        command.Add("-v");
      }
    }

    if (arguments is not [])
    {
      command.AddRange(arguments);
    }

    return command;
  }
}
