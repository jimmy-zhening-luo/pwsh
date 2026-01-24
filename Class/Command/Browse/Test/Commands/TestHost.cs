namespace Module.Command.Browse.Test.Commands;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Host",
  DefaultParameterSetName = "CommonTCPPort",
  HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
)]
[Alias("tn")]
[OutputType(typeof(object))]
public sealed class TestHost : CoreCommand
{
  [Parameter(
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The hostname or IP address of the target host."
  )]
  public string[] Name
  {
    get => names;
    set => names = value;
  }
  private string[] names = [];

  [Parameter(
    ParameterSetName = "CommonTCPPort",
    Position = 1,
    HelpMessage = "Specifies the common service TCP port number."
  )]
  [EnumCompletions(
    typeof(TestHostWellKnownPort)
  )]
  public string? CommonTCPPort;

  [Parameter(
    ParameterSetName = "RemotePort",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The port number to test on the target host."
  )]
  [Alias("RemotePort")]
  [ValidateRange(1, 65535)]
  public ushort Port;

  [Parameter(
    HelpMessage = "The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet."
  )]
  [EnumCompletions(
    typeof(TestHostVerbosity)
  )]
  public TestHostVerbosity InformationLevel;

  [Parameter]
  public SwitchParameter Detailed
  {
    get => detailed;
    set => detailed = value;
  }
  private bool detailed;

  protected sealed override void BeginProcessing()
  {
    if (detailed)
    {
      InformationLevel = TestHostVerbosity.Detailed;
    }
  }

  protected sealed override void ProcessRecord()
  {
    foreach (string name in names)
    {
      if (ParameterSetName == "RemotePort")
      {
        WriteTestNetConnection(
          name,
          string.Empty,
          Port
        );

        continue;
      }

      if (
        !IsPresent(
          "CommonTCPPort"
        )
      )
      {
        WriteTestNetConnection(
          name
        );

        continue;
      }

      if (
        ushort.TryParse(
          CommonTCPPort,
          out var parsedPortNumber
        )
      )
      {
        WriteTestNetConnection(
          name,
          string.Empty,
          parsedPortNumber
        );
      }
      else if (
        Enum.TryParse(
          CommonTCPPort,
          true,
          out TestHostWellKnownPort parsedPortEnum
        )
      )
      {
        WriteTestNetConnection(
          name,
          parsedPortEnum.ToString()
        );
      }
      else
      {
        WriteTestNetConnection(
          name
        );
      }
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    if (names.Length == 0)
    {
      WriteTestNetConnection(
        "google.com",
        string.Empty,
        0
      );
    }
  }

  private void WriteTestNetConnection(
    string computerName,
    string commonTcpPort = "",
    ushort portNumber = 0
  )
  {
    using var ps = CreatePS();

    AddCommand(
      ps,
      "Test-NetConnection",
      CommandTypes.Function
    )
      .AddParameter(
        "ComputerName",
        computerName
      )
      .AddParameter(
        "InformationLevel",
        InformationLevel
      );

    if (portNumber != 0)
    {
      ps.AddParameter(
        "Port",
        portNumber
      );
    }
    else if (
      !string.IsNullOrEmpty(
        commonTcpPort
      )
    )
    {
      ps.AddParameter(
        "CommonTCPPort",
        commonTcpPort
      );
    }

    WriteObject(
      ps.Invoke(),
      true
    );
  }
}
