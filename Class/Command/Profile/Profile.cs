namespace Module.Command.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "Profile"
)]
[Alias("op")]
[OutputType(typeof(void))]
public sealed class StartProfile : CoreCommand
{
  private protected sealed override bool SkipSsh => true;

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
