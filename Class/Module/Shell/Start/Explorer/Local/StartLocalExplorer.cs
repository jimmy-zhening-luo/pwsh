namespace Module.Shell.Start.Explorer.Local
{
  using System.IO;
  using System.Management.Automation;

  public abstract class StartLocalExplorerCommand : LocalWrappedCommand
  {
    [Parameter(
      ParameterSetName = "Path",
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyCollection]
    [SupportsWildcards]
    public string[] Path;

    [Parameter]
    [SupportsWildcards]
    public string Filter;

    [Parameter]
    [SupportsWildcards]
    public string[] Include;

    [Parameter]
    [SupportsWildcards]
    public string[] Exclude;

    protected override string WrappedCommandName() => "Invoke-Item";

    protected override bool NoSsh
    {
      get => true;
    }

    protected override bool BeforeBeginProcessing()
    {
      if (IsPresent("Path"))
      {
        string[] paths = (string[])BoundParameters()["Path"];

        for (int i = 0; i < paths.Length; i++)
        {
          paths[i] = Reanchor(paths[i]);
        }

        BoundParameters()["Path"] = paths;
      }
      else
      {
        BoundParameters()["Path"] = new string[] { Reanchor() };
      }

      return true;
    }
  }
}
