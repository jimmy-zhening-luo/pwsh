namespace Auxiliary.Cli.Commands
{
  using System.Management.Automation;

  [Cmdlet(
    VerbsLifecycle.Invoke,
    "YouTubeDirectory"
  )]
  [Alias("yte")]
  [OutputType(typeof(void))]
  public class InvokeYouTubeDirectory : Cmdlet
  {
    protected override void EndProcessing()
    {
      Core.Context.CreateProcess(
        System.IO.Path.Combine(
          @"Videos\YouTube",
          System.Environment.GetFolderPath(
            System
              .Environment
              .SpecialFolder
              .UserProfile
          )
        )
      );
    }
  }
}
