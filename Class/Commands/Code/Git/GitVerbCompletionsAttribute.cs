namespace Module.Commands.Code.Git;

public sealed class GitVerbCompletionsAttribute() : CompletionsAttribute(
  [
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
    "reset"
  ],
  false,
  Tab.CompletionCase.Preserve
)
{ }
