namespace PowerModule.Commands.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = ParameterSetICMP,
  HelpUri = $"https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
sealed public class TestHost() : WrappedCommand(
  @"NetTCPIP\Test-NetConnection",
  CommandTypes.Function
)
{
  const string ParameterSetICMP = "ICMP";
  const string ParameterSetRemotePort = "RemotePort";
  const string ParameterSetDiagnostics = "NetRouteDiagnostics";

  const string DefaultHost = "google.com";

  public enum Verbosity
  {
    quiet,
    detailed,
  }

  sealed override private protected PipelineInputSource PipelineInput => () => (
    nameof(ComputerName),
    ComputerName
  );

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    [nameof(Detailed)] = default,
    [nameof(InformationLevel)] = Detailed
      ? Verbosity.detailed
      : InformationLevel,
  };

  [Parameter(
    Position = default,
    ValueFromPipeline = true
  )]
  [Alias(
    "Name",
    "RemoteAddress",
    "cn"
  )]
  [ValidateNotNullOrWhiteSpace]
  public string ComputerName
  {
    get => field ?? DefaultHost;
    set;
  }

  [Parameter(
    ParameterSetName = nameof(CommonTCPPort),
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
    ParameterSetName = ParameterSetRemotePort,
    Mandatory = true
  )]
  [Alias(ParameterSetRemotePort)]
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
    ParameterSetName = ParameterSetICMP
  )]
  public SwitchParameter TraceRoute
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetICMP
  )]
  [ValidateRange(1, 120)]
  public int Hops
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetDiagnostics,
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetDiagnostics
  )]
  required public string ConstrainSourceAddress
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = ParameterSetDiagnostics
  )]
  public uint ConstrainInterface
  {
    init => _ = value;
  }

  sealed override private protected void TransformParameters()
  {
    if (
      ParameterSetName is nameof(CommonTCPPort)
      && ushort.TryParse(
        CommonTCPPort,
        out var numericPort
      )
    )
    {
      RemoveBoundParameter(
        nameof(CommonTCPPort)
      );
      SetBoundParameter(
        nameof(Port),
        (int)numericPort
      );
    }
  }
}
