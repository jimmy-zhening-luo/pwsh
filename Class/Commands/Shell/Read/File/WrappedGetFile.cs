namespace PowerModule.Commands.Shell.Read.File;

abstract public class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  abstract public Collection<string> Path
  { init; }
  private protected Collection<string> paths = [];

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  { private get; init; }

  [Parameter]
  public long ReadCount
  { private get; init; }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long TotalCount
  { private get; init; }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public int Tail
  { private get; init; }

  [Parameter]
  required public string Delimiter
  { private get; init; }

  [Parameter]
  required public string Stream
  { private get; init; }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  { private get; init; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  { private get; init; }

  [Parameter]
  public SwitchParameter AsByteStream
  { private get; init; }

  [Parameter]
  public SwitchParameter Raw
  { private get; init; }

  [Parameter]
  public SwitchParameter Wait
  { private get; init; }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = paths = ReanchorPath(paths);
    }
  }
}
