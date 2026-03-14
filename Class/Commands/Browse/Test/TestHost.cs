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
  "ComputerName",
  CommandTypes.Function
)
{
  public enum Verbosity
  {
    quiet,
    detailed,
  }

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
  { get; init; } = string.Empty;

  [Parameter(
    ParameterSetName = "CommonTCPPort",
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
  public ushort Port
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
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  [ValidateRange(1, 120)]
  public int Hops
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  required public string ConstrainSourceAddress
  {
    init => Discard();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface
  {
    init => Discard();
  }

  sealed override private protected void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "RemotePort":
        SetBoundParameter(
          "Port",
          (int)Port
        );

        break;

      case "CommonTCPPort" when ushort.TryParse(
        CommonTCPPort,
        out var numericPort
      ):
        RemoveBoundParameter("CommonTCPPort");
        SetBoundParameter(
          "Port",
          (int)numericPort
        );

        break;

      default:
        break;
    }
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (ComputerName is "")
    {
      SetBoundParameter(
        "ComputerName",
        "google.com"
      );
    }
  }
}
