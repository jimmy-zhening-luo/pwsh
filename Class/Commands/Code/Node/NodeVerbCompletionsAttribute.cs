namespace Module.Commands.Code.Node;

public sealed class NodeVerbCompletionsAttribute : VerbCompletionsAttribute
{
  public NodeVerbCompletionsAttribute() : base(
    typeof(NodeVerb)
  )
  { }
}
