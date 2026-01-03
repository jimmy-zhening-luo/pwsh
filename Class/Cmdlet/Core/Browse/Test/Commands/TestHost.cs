namespace Core.Browse.Test.Commands
{
  using System;
  using System.Management.Automation;
  using Completer;

  [Cmdlet(
    VerbsDiagnostic.Test,
    "Host",
    DefaultParameterSetName = "CommonTCPPort",
    HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
  )]
  [Alias("tn")]
  [OutputType(typeof(Object))]
  public class TestHost : PSCoreCommand
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
    public string CommonTCPPort
    {
      get => commonPort;
      set => commonPort = value;
    }
    private string commonPort = string.Empty;

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

    protected override void BeginProcessing()
    {
      if (detailed)
      {
        verbosity = TestHostVerbosity.Detailed;
      }
    }

    protected override void ProcessRecord()
    {
      foreach (string name in names)
      {
        string portString = string.Empty;
        ushort portNumber = 0;

        if (ParameterSetName == "RemotePort")
        {
          portNumber = port;
        }
        else
        {
          if (commonPort != string.Empty)
          {
            if (
              ushort.TryParse(
                commonPort,
                out var parsedPortNumber
              )
            )
            {
              portNumber = parsedPortNumber;
            }
            else if (
              Enum.TryParse<TestHostWellKnownPort>(
                commonPort,
                true,
                out var parsedPortEnum
              )
            )
            {
              portString = parsedPortEnum.ToString();
            }
          }
        }

        WriteTestNetConnection(
          name,
          portString,
          portNumber
        );
      }
    }

    protected override void EndProcessing()
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
      string commonTcpPort,
      ushort portNumber
    )
    {
      using var ps = PowerShell.Create(
        RunspaceMode.CurrentRunspace
      );
      ps
        .AddCommand(
          SessionState
            .InvokeCommand
            .GetCommand(
              "Test-NetConnection",
              CommandTypes.Function
            )
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
      else
      {
        if (commonTcpPort != string.Empty)
        {
          ps.AddParameter(
            "CommonTCPPort",
            commonTcpPort
          );
        }
      }

      WriteObject(
        ps.Invoke(),
        true
      );
    }
  }
}
