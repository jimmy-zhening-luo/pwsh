namespace Module.Command.Profile;

[Cmdlet(
  VerbsLifecycle.Start,
  "Profile"
)]
[Alias("op")]
[OutputType(typeof(void))]
public class StartProfile : CoreCommand
{
  private protected sealed override void AfterEndProcessing()
  {
    var repoRootVariable = Var(
      "REPO_ROOT"
    );

    if (
      repoRootVariable is PSObject repoRootObject
      && repoRootObject.BaseObject is string repoRoot
    )
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
