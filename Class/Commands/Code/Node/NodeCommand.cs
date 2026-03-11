namespace PowerModule.Commands.Code.Node;

abstract public class NodeCommand(string? IntrinsicVerb) : CodeNativeCommand(IntrinsicVerb)
{
  sealed private protected class NodeVerbCompletionsAttribute() : Tab.CompletionsAttribute<HashSet<string>>(Verbs);

  static readonly HashSet<string> Verbs = [
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

  static readonly Dictionary<string, string> Aliases = new()
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

  sealed override private protected string CommandPath
  { get; } = Client.Environment.Application.Npm;

  override private protected SwitchBoard Uppercase
  { get; } = new(
    D: true,
    E: true,
    P: true
  );

  sealed override private protected IEnumerable<string> CommandBaseArguments
  { get; } = ["--color=always"];

  sealed override private protected IEnumerable<string> WorkingDirectoryArguments => WorkingDirectory is ""
    ? []
    : [
        $"--prefix={Pwd(WorkingDirectory)}",
      ];

  sealed override private protected void PreprocessIntrinsicVerb()
  {
    switch (IntrinsicVerb)
    {
      case null:
        break;

      case var verb when Aliases.TryGetValue(
        verb.ToLower(
          Client.StringInput.CurrentCulture
        ),
        out var alias
      ):
        IntrinsicVerb = alias;
        break;

      case var verb when Verbs.TryGetValue(
        verb.ToLower(
          Client.StringInput.CurrentCulture
        ),
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;
        break;

      default:
        break;
    }
  }

  sealed override private protected void PreprocessWorkingDirectory()
  {
    switch (WorkingDirectory)
    {
      case "":
        break;

      case var path when !IsNodePackage(path):
        DeferredVerbArguments.Add(path);
        WorkingDirectory = string.Empty;
        break;

      case var path when Pwd(path) == Pwd():
        WorkingDirectory = string.Empty;
        break;

      default:
        break;
    }
  }

  private protected bool IsNodePackage(string path) => System.IO.File.Exists(
    System.IO.Path.Combine(
      Pwd(path),
      "package.json"
    )
  );
}
