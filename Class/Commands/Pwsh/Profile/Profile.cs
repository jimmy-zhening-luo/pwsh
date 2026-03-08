namespace Module.Commands.Pwsh.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "Profile"
)]
[Alias("op")]
[OutputType(typeof(void))]
sealed public class StartProfile() : CoreCommand(true)
{
  sealed private protected override void Postprocess()
  {
    Client.Start.CreateProcess(
      Client.Environment.Known.Application.VSCode,
      Client.Environment.Known.Folder.Code("pwsh")
    );
  }
}
