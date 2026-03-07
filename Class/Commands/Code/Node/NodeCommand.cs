namespace Module.Commands.Code.Node;

public abstract class NodeCommand(string? IntrinsicVerb) : RemoteNativeVerbCommand(IntrinsicVerb)
{
  private protected sealed class NodeVerbCompletionsAttribute() : Tab.CompletionsAttribute([.. Verbs]);

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

  private protected sealed override string CommandPath { get; } = Client.Environment.Known.Application.Npm;

  private protected override SwitchBoard Uppercase { get; set; } = new(
    D: true,
    E: true,
    P: true
  );

  private protected sealed override List<string> NativeCommandArguments()
  {
    List<string> command = ["--color=always"];

    switch (IntrinsicVerb)
    {
      case null:
        break;

      case var verb when Aliases.TryGetValue(
        verb.ToLower(),
        out var alias
      ):
        IntrinsicVerb = alias;
        break;

      case var verb when Verbs.TryGetValue(
        verb.ToLower(),
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;
        break;
    }

    switch (WorkingDirectory)
    {
      case "":
        break;

      case var path when !IsNodePackage(path):
        DeferredVerbArguments.Add(path);
        break;

      case var path when Pwd(path) is var fullPath
        && fullPath != Pwd():
        command.Add(
          $"--prefix={Pwd(fullPath)}"
        );
        break;
    }

    return command;
  }

  private protected bool IsNodePackage(string path) => System.IO.File.Exists(
    System.IO.Path.Combine(
      Pwd(path),
      "package.json"
    )
  );
}
