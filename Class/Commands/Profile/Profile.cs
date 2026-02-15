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
    Invocation.CreateProcess(
      Application.VSCode,
      Code(
        "pwsh"
      ),
      true
    );
  }
}
