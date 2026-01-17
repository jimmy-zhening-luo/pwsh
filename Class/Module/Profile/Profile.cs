namespace Module.Profile
{
  namespace Commands
  {
    using System.Management.Automation;

    [Cmdlet(
      VerbsLifecycle.Start,
      "Profile"
    )]
    [Alias("op")]
    [OutputType(typeof(void))]
    public class StartProfile : CoreCommand
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
}
