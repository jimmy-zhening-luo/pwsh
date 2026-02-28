namespace Module.Commands.Browse.Test.Host;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = "ICMP",
  HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
public sealed class TestHost() : WrappedCommand(
  @"NetTCPIP\Test-NetConnection",
  CommandType: CommandTypes.Function
)
{
  public enum TestHostVerbosity
  {
    Quiet,
    Detailed
  }

  internal enum TestHostWellKnownPort
  {
    HTTP = -4,
    RDP,
    SMB,
    WINRM
  }

  [Parameter(
    ParameterSetName = "ICMP",
    Position = default
  )]
  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Position = default
  )]
  [Parameter(
    ParameterSetName = "RemotePort",
    Position = default
  )]
  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Position = default
  )]
  [Alias(
    "Name",
    "RemoteAddress",
    "cn"
  )]
  public string ComputerName { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Mandatory = true,
    Position = 1
  )]
  [ValidateNotNullOrWhiteSpace]
  [EnumCompletions(typeof(TestHostWellKnownPort))]
  public string CommonTCPPort { get; set; } = string.Empty;

  [Parameter(
    ParameterSetName = "RemotePort",
    Mandatory = true
  )]
  [Alias("RemotePort")]
  [ValidateRange(1, 65535)]
  public ushort Port { get; set; }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  public SwitchParameter TraceRoute { get; set; }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  [ValidateRange(1, 120)]
  public int Hops { get; set; }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting { get; set; }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public required string ConstrainSourceAddress { get; set; }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface { get; set; }

  [Parameter]
  [EnumCompletions(typeof(TestHostVerbosity))]
  public TestHostVerbosity InformationLevel { get; set; }

  [Parameter]
  public SwitchParameter Detailed
  {
    get => detailed;
    set => detailed = value;
  }
  private bool detailed;

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["Detailed"] = default,
    ["InformationLevel"] = detailed
      ? TestHostVerbosity.Detailed
      : InformationLevel,
  };

  private protected sealed override void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "ICMP" when ComputerName is "":
        CommonTCPPort = nameof(TestHostWellKnownPort.HTTP);
        BoundParameters["CommonTCPPort"] = CommonTCPPort;

        break;
      case "RemotePort":
        BoundParameters["Port"] = (int)Port;

        break;
      case "CommonTCPPort":
        if (
          ushort.TryParse(
            CommonTCPPort,
            out var parsedPortNumber
          )
        )
        {
          CommonTCPPort = string.Empty;
          _ = BoundParameters.Remove("CommonTCPPort");

          Port = parsedPortNumber;
          BoundParameters["Port"] = (int)Port;
        }
        else if (
          System.Enum.TryParse(
            CommonTCPPort,
            true,
            out TestHostWellKnownPort parsedPortEnum
          )
        )
        {
          CommonTCPPort = parsedPortEnum.ToString();
          BoundParameters["CommonTCPPort"] = CommonTCPPort;
        }

        break;
      default:
        break;
    }
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (ComputerName is "")
    {
      ComputerName = "google.com";
      BoundParameters["ComputerName"] = ComputerName;
    }
  }
}
