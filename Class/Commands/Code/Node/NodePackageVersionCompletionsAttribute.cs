namespace Module.Commands.Code.Node;

public sealed class NodePackageVersionCompletionsAttribute() : EnumCompletionsAttribute(
  typeof(NodePackageVersion),
  Tab.CompletionCase.Preserve
)
{ }
