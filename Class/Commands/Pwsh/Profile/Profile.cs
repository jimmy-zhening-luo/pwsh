namespace PowerModule.Commands.Pwsh.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "Profile"
)]
[Alias("op")]
[OutputType(typeof(void))]
sealed public class StartProfile() : CoreCommand(true)
{
  sealed override private protected void Postprocess() => Client.File.Handler.Edit(
    Client.Environment.Known.Folder.Code("pwsh")
  );
}
