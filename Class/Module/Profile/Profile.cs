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
        @"Programs\Microsoft VS Code\bin\code.cmd"
      ),
      System.IO.Path.GetFullPath(
        "pwsh",
        ((PSObject)Var("REPO_ROOT")).BaseObject.ToString()
      )
        + " --profile=Default",
      true
    );
  }
}
