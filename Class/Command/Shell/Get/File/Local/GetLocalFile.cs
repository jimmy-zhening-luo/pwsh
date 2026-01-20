namespace Module.Command.Shell.Get.File.Local;

using System.Text;

public abstract class GetLocalFileCommand : LocalWrappedCommand
{
  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  public string[]? Path;

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [SupportsWildcards]
  public string? Filter;

  [Parameter]
  [SupportsWildcards]
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  [Parameter(
    ValueFromPipelineByPropertyName = true
  )]
  public long? ReadCount;

  [Parameter(
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("First", "Head")]
  [ValidateRange(0, 9223372036854775807)]
  public long? TotalCount;

  [Parameter(
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("Last")]
  [ValidateRange(0, 2147483647)]
  public int? Tail;

  [Parameter]
  public string? Delimiter;

  [Parameter]
  public string? Stream;

  [Parameter]
  public Encoding? Encoding;

  [Parameter]
  [Alias("f")]
  public SwitchParameter? Force;

  [Parameter]
  public SwitchParameter? AsByteStream;

  [Parameter]
  public SwitchParameter? Raw;

  [Parameter]
  public SwitchParameter? Wait;

  protected override string WrappedCommandName => "Get-Content";

  protected override bool BeforeBeginProcessing()
  {
    if (IsPresent("Path"))
    {
      string[] paths = (string[])BoundParameters["Path"];

      for (int i = 0; i < paths.Length; i++)
      {
        paths[i] = Reanchor(paths[i]);
      }

      BoundParameters["Path"] = paths;
    }
    else
    {
      BoundParameters["Path"] = new string[] { Reanchor() };
    }
    return true;
  }
}
