namespace Module.Command.Shell.Set.Directory;

public abstract class WrappedSetDirectory : WrappedCommand
{
  private protected WrappedSetDirectory() : base(
    "Set-Location"
  )
  { }

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

  private protected sealed override void AnchorBoundPath()
  {
    BoundParameters["Path"] = Reanchor(
      BoundParameters.TryGetValue(
        "Path",
        out var path
      )
        ? path?.ToString() ?? string.Empty
        : string.Empty
    );
  }

  private protected sealed override bool BeforeBeginProcessing()
  {
    if (
      Here
      && !IsPresent("Path")
      && !IsPresent("LiteralPath")
    )
    {
      string pwd = Pwd();
      string parent = IO.Path.GetFullPath(
        "..",
        pwd
      );

      BoundParameters["Path"] = parent == pwd
        ? Home()
        : parent;
    }

    return true;
  }
}
