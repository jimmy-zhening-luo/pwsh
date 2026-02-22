namespace Module.Commands.Code.Node;

public sealed class NodeVerbCompletionsAttribute() : CompletionsAttribute(
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
  CompletionCase.Preserve
)
{ }
