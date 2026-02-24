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
    get => name;
    set => name = value;
  }
  private string name = "";

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
    set => commonPort = value;
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
    get => port;
    set => port = value;
  }
  private ushort port;

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
    get => hops;
    set => hops = value;
  }
  private int hops;

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
    get => constrainSourceAddress;
    set => constrainSourceAddress = value;
  }
  private string constrainSourceAddress = "";

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface
  {
    get => constrainInterface;
    set => constrainInterface = value;
  }
  private uint constrainInterface;

  [Parameter]
  [EnumCompletions(
    typeof(TestHostVerbosity)
  )]
  public TestHostVerbosity InformationLevel
  {
    get => verbosity;
    set => verbosity = value;
  }
  private TestHostVerbosity verbosity;

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
      verbosity = TestHostVerbosity.Detailed;
    }

    MyInvocation.BoundParameters.Remove("Detailed");
    MyInvocation.BoundParameters["InformationLevel"] = verbosity.ToString();

    switch (ParameterSetName)
    {
      case "ICMP":
        if (
          string.IsNullOrEmpty(
            name
          )
        )
        {
          MyInvocation.BoundParameters["CommonTCPPort"] = TestHostWellKnownPort.HTTP.ToString();
        }

        break;
      case "RemotePort":
        MyInvocation.BoundParameters["Port"] = (int)port;

        break;
      case "CommonTCPPort":
        MyInvocation.BoundParameters.Remove("CommonTCPPort");

        if (
          ushort.TryParse(
            commonPort,
            out var parsedPortNumber
          )
        )
        {
          MyInvocation.BoundParameters["Port"] = (int)parsedPortNumber;
        }
        else if (
          System.Enum.TryParse(
            commonPort,
            true,
            out TestHostWellKnownPort parsedPortEnum
          )
        )
        {
          MyInvocation.BoundParameters["CommonTCPPort"] = parsedPortEnum.ToString();
        }

        break;
      default:
        break;
    }

    if (
      string.IsNullOrEmpty(
        name
      )
    )
    {
      name = "google.com";
      MyInvocation.BoundParameters["ComputerName"] = name;
    }
  }
}
