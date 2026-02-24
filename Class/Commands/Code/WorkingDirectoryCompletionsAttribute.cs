namespace Module.Commands.Code;

public sealed class WorkingDirectoryCompletionsAttribute() : PathCompletionsAttribute(
  @"~\",
  PathItemType.Directory,
  true
)
{ }
