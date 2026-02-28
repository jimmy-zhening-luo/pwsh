namespace Module.Commands.Code.Node;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = "https://docs.npmjs.com/cli/commands"
)]
[Alias("n")]
public sealed class NpmInvoke : CoreCommand
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
    Position = 1,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional npm arguments"
  )]
  public string[] ArgumentList { get; set; } = [];

  [Parameter(
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error."
  )]
  public SwitchParameter NoThrow
  {
    get => noThrow;
    set => noThrow = value;
  }
  private bool noThrow;

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as npm argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => version;
    set => version = value;
  }
  private bool version;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -D flag as npm argument"
  )]
  public SwitchParameter D
  {
    get => d;
    set => d = value;
  }
  private bool d;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -E flag as npm argument"
  )]
  public SwitchParameter E
  {
    get => e;
    set => e = value;
  }
  private bool e;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as npm argument"
  )]
  public SwitchParameter I
  {
    get => i;
    set => i = value;
  }
  private bool i;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as npm argument"
  )]
  public SwitchParameter O
  {
    get => o;
    set => o = value;
  }
  private bool o;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -P flag as npm argument"
  )]
  public SwitchParameter P
  {
    get => p;
    set => p = value;
  }
  private bool p;

  private protected sealed override void Postprocess()
  {
    List<string> nodeCommand = [
      "&",
      "npm.ps1",
      "--color=always",
    ];

    List<string> nodeArguments = [];

    if (ArgumentList is not [])
    {
      nodeArguments.AddRange(ArgumentList);
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
          nodeCommand.Add(packagePrefix);
        }
      }
      else
      {
        nodeArguments.Insert(default, WorkingDirectory);

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
      var deferredVerb = nodeArguments is []
        ? ""
        : nodeArguments.Find(
            argument => Verbs.Contains(
              argument.ToLower()
            )
          );

      if (deferredVerb is not (null or ""))
      {
        _ = nodeArguments.Remove(deferredVerb);
      }

      nodeArguments.Insert(default, Verb);

      Verb = deferredVerb ?? string.Empty;
    }

    if (Verb is not "")
    {
      nodeCommand.Add(Verb.ToLower());

      if (d)
      {
        nodeArguments.Add("-D");
      }
      if (e)
      {
        nodeArguments.Add("-E");
      }
      if (i)
      {
        nodeArguments.Add("-i");
      }
      if (o)
      {
        nodeArguments.Add("-o");
      }
      if (p)
      {
        nodeArguments.Add("-P");
      }
    }
    else
    {
      if (version)
      {
        nodeCommand.Add("-v");
      }
    }

    if (nodeArguments is not [])
    {
      nodeCommand.AddRange(nodeArguments);
    }

    List<string> escapedNodeCommand = [];

    foreach (var word in nodeCommand)
    {
      escapedNodeCommand.Add(
        Client.Console.String.EscapeDoubleQuoted(word)
      );
    }

    AddScript(string.Join(' ', escapedNodeCommand));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      if (noThrow)
      {
        WriteWarning("npm error");
      }
      else
      {
        Throw("npm error");
      }
    }
  }
}
