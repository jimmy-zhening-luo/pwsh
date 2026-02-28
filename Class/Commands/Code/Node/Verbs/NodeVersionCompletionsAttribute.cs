namespace Module.Commands.Code.Node.Verbs;

public sealed class NodeVersionCompletionsAttribute() : EnumCompletionsAttribute(
  typeof(NodeUpdateVersion.NodeVersion),
  default,
  ["from-git"],
  ["prepatch"]
);
