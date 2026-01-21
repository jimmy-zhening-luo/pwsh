namespace Module.Command.Profile;

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
    var repoRootVariable = Var(
      "REPO_ROOT"
    )
      .BaseObject;

    if (repoRootVariable is string repoRoot)
    {
      CreateProcess(
        LocalAppData(
          @"Programs\Microsoft VS Code\bin\code.cmd"
        ),
        Path.GetFullPath(
          "pwsh",
          repoRoot
        )
          + " --profile=Default",
        true
      );
    }
    else
    {
      Throw(
        "Expected global Powershell variable '$REPO_ROOT' was not found",
        "PSProfileGlobalVariableNotSet",
        ErrorCategory.ObjectNotFound,
        repoRootVariable
      );
    }
  }
}
