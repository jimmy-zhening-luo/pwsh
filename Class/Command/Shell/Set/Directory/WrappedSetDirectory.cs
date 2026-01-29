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
  [SupportsWildcards]
  public string? Path;

  [Parameter]
  public SwitchParameter? PassThru;

  private protected sealed override void TransformParameters()
  {
    if (Here)
    {
      if (
        !IsPresent(
          "Path"
        )
        && !IsPresent(
          "LiteralPath"
        )
      )
      {
        string pwd = Pwd();
        string parent = GetFullPath(
          "..",
          pwd
        );

        BoundParameters["Path"] = parent == pwd
          ? Home()
          : parent;
      }
    }
    else
    {
      BoundParameters["Path"] = Reanchor(
        BoundParameters.TryGetValue(
          "Path",
          out var path
        )
          ? path?.ToString()
            ?? string.Empty
          : string.Empty
      );
    }
  }
}
