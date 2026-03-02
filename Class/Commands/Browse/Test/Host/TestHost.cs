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
    [System.ComponentModel.Description("Only display the boolean result of whether the host is reachable (Default)")]
    quiet,

    [System.ComponentModel.Description("Display the full output of Test-NetConnection, which includes diagnostic information such as round-trip time and IP address information")]
    detailed,
  }

  private enum TestHostWellKnownPort
  {
    [System.ComponentModel.Description("Hypertext Transfer Protocol (HTTP)")]
    HTTP = -4,

    [System.ComponentModel.Description("Remote Desktop Protocol (RDP)")]
    RDP,

    [System.ComponentModel.Description("Server Message Block (SMB)")]
    SMB,

    [System.ComponentModel.Description("Windows Remote Management (WinRM)")]
    WINRM,
  }

  [Parameter(Position = default)]
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
  [Tab.Completer.EnumCompletions(typeof(TestHostWellKnownPort))]
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
      ? TestHostVerbosity.detailed
      : InformationLevel,
  };

  private protected sealed override void TransformArguments()
  {
    switch (ParameterSetName)
    {
      case "ICMP" when ComputerName is "":
        BoundParameters["CommonTCPPort"] = CommonTCPPort = nameof(TestHostWellKnownPort.HTTP);
        break;

      case "RemotePort":
        BoundParameters["Port"] = (int)Port;
        break;

      case "CommonTCPPort":
        switch (CommonTCPPort)
        {
          case var port when ushort.TryParse(port, out var numericPort):
            _ = BoundParameters.Remove("CommonTCPPort");
            CommonTCPPort = string.Empty;
            BoundParameters["Port"] = (int)(Port = numericPort);
            break;

          case var port when System.Enum.TryParse<TestHostWellKnownPort>(
            port,
            true,
            out var commonPort
          ):
            BoundParameters["CommonTCPPort"] = CommonTCPPort = commonPort.ToString();
            break;
        }
        break;
    }
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (ComputerName is "")
    {
      BoundParameters["ComputerName"] = ComputerName = "google.com";
    }
  }
}
