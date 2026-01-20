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
      protected override void EndProcessing()
      {
        var repoRoot = Var("REPO_ROOT")
          .BaseObject
          .ToString();

        if (repoRoot == null)
        {
          Throw(
            "REPO_ROOT environment variable is not set.",
            "PSProfileGlobalVariableNotSet",
            ErrorCategory.ResourceUnavailable
          );
        }

        CreateProcess(
          LocalAppData(
            @"Programs\Microsoft VS Code\bin\code.cmd"
          ),
          System.IO.Path.GetFullPath(
            "pwsh",
            repoRoot
          )
            + " --profile=Default",
          true
        );
      }
    }
  }
}
