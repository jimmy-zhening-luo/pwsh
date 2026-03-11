namespace PowerModule.Commands.Browse.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = "ICMP",
  HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
sealed public class TestHost() : WrappedCommand(
  @"NetTCPIP\Test-NetConnection",
  AcceptsPipelineInput: true,
  CommandTypes.Function
)
{
  public enum TestHostVerbosity
  {
    [System.ComponentModel.Description(
      "Display only whether or not the host is reachable (Default)"
    )]
    quiet,

    [System.ComponentModel.Description(
      "Display the full output of Test-NetConnection, including diagnostic information such as round-trip time and IP address resolution"
    )]
    detailed,
  }

  enum TestHostWellKnownPort
  {
    [System.ComponentModel.Description(
      "Hypertext Transfer Protocol (HTTP)"
    )]
    HTTP = -4,

    [System.ComponentModel.Description(
      "Remote Desktop Protocol (RDP)"
    )]
    RDP,

    [System.ComponentModel.Description(
      "Server Message Block (SMB)"
    )]
    SMB,

    [System.ComponentModel.Description(
      "Windows Remote Management (WinRM)"
    )]
    WINRM,
  }

  sealed override private protected object? PipelineInput => ComputerName;

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    ["Detailed"] = default,
    ["InformationLevel"] = Detailed
      ? TestHostVerbosity.detailed
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
  { get; init; } = string.Empty;

  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Mandatory = true,
    Position = 1
  )]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(
    typeof(TestHostWellKnownPort),
    Tab.CompletionCase.Lower
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
  public TestHostVerbosity InformationLevel
  { private get; init; }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  public SwitchParameter TraceRoute
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "ICMP"
  )]
  [ValidateRange(1, 120)]
  public int Hops
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics",
    Mandatory = true
  )]
  public SwitchParameter DiagnoseRouting
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  required public string ConstrainSourceAddress
  {
    init => Bind();
  }

  [Parameter(
    ParameterSetName = "NetRouteDiagnostics"
  )]
  public uint ConstrainInterface
  {
    init => Bind();
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

      case "CommonTCPPort":
        switch (CommonTCPPort)
        {
          case var port when ushort.TryParse(
            port,
            out var numericPort
          ):
            RemoveBoundParameter("CommonTCPPort");
            SetBoundParameter(
              "Port",
              (int)numericPort
            );
            break;

          case var port when System.Enum.TryParse<TestHostWellKnownPort>(
            port,
            true,
            out var commonPort
          ):
            SetBoundParameter(
              "CommonTCPPort",
              commonPort.ToString()
            );
            break;

          default:
            break;
        }

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
