namespace Module.Commands.Code;

public sealed class WorkingDirectoryCompletionsAttribute : PathCompletionsAttribute
{
  public WorkingDirectoryCompletionsAttribute() : base(
    @"~\code",
    PathItemType.Directory
  )
  {
    Flat = true;
  }
}
