namespace Module.Commands;

public abstract partial class CoreCommand
{
  private enum CommandLifecycle
  {
    NotStarted,
    Initialized,
    Processing,
    Skipped,
    Stopped
  }
}