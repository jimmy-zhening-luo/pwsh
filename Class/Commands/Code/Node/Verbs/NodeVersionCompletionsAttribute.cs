namespace Module.Commands.Code.Node.Verbs;

internal sealed class NodeVersionCompletionsAttribute() : EnumCompletionsAttribute(
  typeof(NodeUpdateVersion.NodeVersion),
  default,
  ["from-git"],
  ["prepatch"]
);
