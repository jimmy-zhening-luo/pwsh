namespace Module.Commands.Code;

internal sealed class WorkingDirectoryCompletionsAttribute : PathCompletionsAttribute
{
  internal WorkingDirectoryCompletionsAttribute() : base(
    @"~\code",
    PathItemType.Directory
  )
  {
    Flat = true;
  }
}
