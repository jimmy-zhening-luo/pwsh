namespace Module.Shell.Set.Directory.Local
{
  using System;
  using System.Management.Automation;

  public abstract class SetLocalDirectoryCommand : LocalWrappedCommand
  {
    [Parameter(
      ParameterSetName = "Path",
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyString]
    [SupportsWildcards]
    public string? Path;

    [Parameter]
    public SwitchParameter? PassThru;

    protected override string WrappedCommandName => "Set-Location";

    protected override bool BeforeBeginProcessing()
    {
      BoundParameters()["Path"] = Reanchor(
        BoundParameters().TryGetValue("Path", out var path)
          ? path?.ToString() ?? string.Empty
          : string.Empty
      );

      return true;
    }
  }
}
