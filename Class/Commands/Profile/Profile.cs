namespace Module.Commands.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "Profile"
)]
[Alias("op")]
[OutputType(typeof(void))]
public sealed class StartProfile() : CoreCommand(
  true
)
{
  private protected sealed override void AfterEndProcessing()
  {
    Client.Invocation.CreateProcess(
      Client.Environment.Known.Application.VSCode,
      Client.Environment.Known.Folder.Code(
        "pwsh"
      ),
      true
    );
  }
}
