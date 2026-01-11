namespace Module.Profile.Commands
{
  using System.Management.Automation;

  [Cmdlet(
    VerbsLifecycle.Start,
    "Profile"
  )]
  [Alias("op")]
  [OutputType(typeof(void))]
  public class StartProfile : PSCoreCommand
  {
    protected override void EndProcessing() => Context.CreateProcess(
      Context.LocalAppData(
        @"\Programs\Microsoft VS Code\bin\code.cmd"
      ),
      System.IO.Path.GetFullPath(
        "pwsh",
        (string)(Var("REPO_ROOT").ToString())
      )
        + " --profile=Default",
      true
    );
  }
}
