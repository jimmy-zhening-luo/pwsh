namespace Module.Commands.Code;

public sealed class WorkingDirectoryCompletionsAttribute() : PathCompletionsAttribute(
  @"~\code",
  PathItemType.Directory,
  true
)
{ }
