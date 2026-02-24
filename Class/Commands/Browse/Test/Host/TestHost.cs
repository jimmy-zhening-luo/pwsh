namespace Module.Commands.Browse.Test.Host;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = "ICMP",
  HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
public sealed partial class TestHost() : WrappedCommand(
  "Test-NetConnection",
  false,
  "",
  CommandTypes.Function
)
{
  [Parameter(
    ParameterSetName = "ICMP",
    Position = 0
  )]
  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Position = 0
  )]
  [Parameter(
    ParameterSetName = "RemotePort",
    Position = 0
  )]
  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Position = 0
  )]
  [Alias(
    "Name",
    "RemoteAddress",
    "cn"
  )]
  public string ComputerName
  {
    get;
    set;
  } = "";

  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Mandatory = true,
    Position = 1
  )]
  [EnumCompletions(
    typeof(TestHostWellKnownPort)
  )]
  public string CommonTCPPort
  {
    get => commonPort;
    set => commonPort;
  }
  private string commonPort = "";

  [Parameter(
    ParameterSetName = "RemotePort",
    Mandatory = true
  )]
  [Alias("RemotePort")]
  [ValidateRange(1, 65535)]
  public ushort Port
  {
    get;
    set;
  }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  public SwitchParameter TraceRoute
  {
    get => traceRoute;
    set => traceRoute = value;
  }
  private bool traceRoute;

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  [ValidateRange(1, 120)]
  public int Hops
  {
    get;
    set;
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting
  {
    get => diagnoseRouting;
    set => diagnoseRouting = value;
  }
  private bool diagnoseRouting;

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public string ConstrainSourceAddress
  {
    get;
    set;
  } = "";

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface
  {
    get;
    set;
  }

  [Parameter]
  [EnumCompletions(
    typeof(TestHostVerbosity)
  )]
  public TestHostVerbosity InformationLevel
  {
    get;
    set;
  }

  [Parameter]
  public SwitchParameter Detailed
  {
    get => detailed;
    set => detailed = value;
  }
  private bool detailed;

  private protected sealed override void TransformParameters()
  {
    if (detailed)
    {
      InformationLevel = TestHostVerbosity.Detailed;
    }

    MyInvocation.BoundParameters.Remove("Detailed");
    MyInvocation.BoundParameters["InformationLevel"] = InformationLevel.ToString();

    switch (ParameterSetName)
    {
      case "ICMP":
        if (
          string.IsNullOrEmpty(
            ComputerName
          )
        )
        {
          CommonTCPPort = TestHostWellKnownPort.HTTP.ToString();
          MyInvocation.BoundParameters["CommonTCPPort"] = CommonTCPPort;
        }

        break;
      case "RemotePort":
        MyInvocation.BoundParameters["Port"] = (int)Port;

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
          MyInvocation.BoundParameters.Remove("CommonTCPPort");

          Port = parsedPortNumber;
          MyInvocation.BoundParameters["Port"] = (int)Port;
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
          MyInvocation.BoundParameters["CommonTCPPort"] = CommonTCPPort;
        }

        break;
      default:
        break;
    }

    if (
      string.IsNullOrEmpty(
        ComputerName
      )
    )
    {
      ComputerName = "google.com";
      MyInvocation.BoundParameters["ComputerName"] = ComputerName;
    }
  }
}
