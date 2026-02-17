namespace Module.Commands.Browse.Test;

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
  public enum TestHostVerbosity
  {
    Quiet,
    Detailed
  }

  public enum TestHostWellKnownPort
  {
    HTTP = -4,
    RDP,
    SMB,
    WINRM
  }

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
  public string CommonTCPPort
  {
    get => commonPort;
    set => commonPort = value;
  }
  private string commonPort = "";

  [Parameter(
    ParameterSetName = "RemotePort",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The port number to test on the target host."
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
    HelpMessage = "The level of information to return, can be Quiet or Detailed. Will not take effect if Detailed switch is set. Defaults to Quiet."
  )]
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
  }

  private protected sealed override bool ValidateParameters() => names.Length != 0;

  private protected sealed override void ProcessRecordAction()
  {
    foreach (string name in names)
    {
      if (ParameterSetName == "RemotePort")
      {
        WriteTestNetConnection(
          name,
          port
        );

        continue;
      }

      if (
        string.IsNullOrEmpty(
          commonPort
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
          parsedPortNumber
        );
      }
      else if (
        System.Enum.TryParse(
          CommonTCPPort,
          true,
          out TestHostWellKnownPort parsedPortEnum
        )
      )
      {
        WriteTestNetConnection(
          name,
          parsedPortEnum
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

  private protected sealed override void DefaultAction() => WriteTestNetConnection(
    "google.com",
    TestHostWellKnownPort.HTTP
  );

  private void WriteTestNetConnection(
    string computerName,
    ushort portNumber = 0
  ) => BuildWriteTestNetConnection(
    computerName,
    string.Empty,
    portNumber
  );

  private void WriteTestNetConnection(
    string computerName,
    TestHostWellKnownPort wellknownPort
  ) => BuildWriteTestNetConnection(
    computerName,
    wellknownPort.ToString()
  );

  private void BuildWriteTestNetConnection(
    string computerName,
    string wellknownPortString = "",
    ushort portNumber = 0
  )
  {
    using var ps = PowerShellHost.Create(
      true
    );

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
        verbosity
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
        wellknownPortString
      )
    )
    {
      ps.AddParameter(
        "CommonTCPPort",
        wellknownPortString
      );
    }

    WriteObject(
      ps.Invoke(),
      true
    );
  }
}
