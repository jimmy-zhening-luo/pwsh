namespace PowerModule.Commands.Shell.Read.File;

abstract public class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  abstract public string[] Path
  { set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  { private get; set; }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  { private get; set; }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  { private get; set; }

  [Parameter]
  public long ReadCount
  { private get; set; }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long TotalCount
  { private get; set; }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public int Tail
  { private get; set; }

  [Parameter]
  required public string Delimiter
  { private get; set; }

  [Parameter]
  required public string Stream
  { private get; set; }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  { private get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  { private get; set; }

  [Parameter]
  public SwitchParameter AsByteStream
  { private get; set; }

  [Parameter]
  public SwitchParameter Raw
  { private get; set; }

  [Parameter]
  public SwitchParameter Wait
  { private get; set; }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = paths = ReanchorPath(paths);
    }
  }
}
