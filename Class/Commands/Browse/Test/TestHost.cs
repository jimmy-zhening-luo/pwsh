namespace PowerModule.Commands.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = "ICMP",
  HelpUri = $"https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
sealed public class TestHost() : WrappedCommand(
  @"NetTCPIP\Test-NetConnection",
  CommandTypes.Function
)
{
  const string CommonPortParameterName = "CommonTCPPort";

  public enum Verbosity
  {
    quiet,
    detailed,
  }

  sealed override private protected PipelineInputSource PipelineInput => () => (
    "ComputerName",
    ComputerName
  );

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    ["Detailed"] = default,
    ["InformationLevel"] = Detailed
      ? Verbosity.detailed
      : InformationLevel,
  };

  [Parameter(
    Position = default,
    ValueFromPipeline = true
  )]
  [Alias(
    StandardParameter.Name,
    "RemoteAddress",
    "cn"
  )]
  [ValidateNotNullOrWhiteSpace]
  public string ComputerName
  {
    get => field ?? "google.com";
    set;
  }

  [Parameter(
    ParameterSetName = CommonPortParameterName,
    Mandatory = true,
    Position = 1
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.Completions(
    "http",
    "rdp",
    "smb",
    "winrm"
  )]
  required public string CommonTCPPort
  { private get; init; }

  [Parameter(
    ParameterSetName = "RemotePort",
    Mandatory = true
  )]
  [Alias("RemotePort")]
  [ValidateRange(ValidateRangeKind.Positive)]
  public int Port
  { private get; init; }

  [Parameter]
  public SwitchParameter Detailed
  { private get; init; }

  [Parameter]
  public Verbosity InformationLevel
  { private get; init; }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  public SwitchParameter TraceRoute
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  [ValidateRange(1, 120)]
  public int Hops
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  required public string ConstrainSourceAddress
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface
  {
    init => _ = value;
  }

  sealed override private protected void TransformParameters()
  {
    if (
      ParameterSetName is CommonPortParameterName
      && ushort.TryParse(
        CommonTCPPort,
        out var numericPort
      )
    )
    {
      RemoveBoundParameter(CommonPortParameterName);
      SetBoundParameter(
        "Port",
        (int)numericPort
      );
    }
  }
}
