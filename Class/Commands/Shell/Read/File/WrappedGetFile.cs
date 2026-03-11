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
  {
    init => Bind();
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  {
    init => Bind();
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  {
    init => Bind();
  }

  [Parameter]
  public long ReadCount
  {
    init => Bind();
  }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long TotalCount
  {
    init => Bind();
  }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public int Tail
  {
    init => Bind();
  }

  [Parameter]
  required public string Delimiter
  {
    init => Bind();
  }

  [Parameter]
  required public string Stream
  {
    init => Bind();
  }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
  {
    init => Bind();
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => Bind();
  }

  [Parameter]
  public SwitchParameter AsByteStream
  {
    init => Bind();
  }

  [Parameter]
  public SwitchParameter Raw
  {
    init => Bind();
  }

  [Parameter]
  public SwitchParameter Wait
  {
    init => Bind();
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      SetBoundParameter(
        "Path",
        ReanchorPath(paths)
      );
    }
  }
}
