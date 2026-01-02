using System;
using System.Diagnostics;
using System.Management.Automation;
using Completer;

namespace Browse
{
  public enum TestHostVerbosity {
    Quiet,
    Detailed
  }
  
  public enum TestHostWellKnownPort {
    HTTP = -4,
    RDP,
    SMB,
    WINRM
  }

  namespace Commands
  {
    [Cmdlet(
      VerbsDiagnostic.Test,
      "Host",
      DefaultParameterSetName = "CommonTCPPort",
      HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
    )]
    [Alias("tn")]
    [OutputType(typeof(Object))]
    public class TestHost : PSCmdlet
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        HelpMessage = "The hostname or IP address of the target host."
      )]
      public string[] Name
      {
        get => name;
        set => name = value;
      }
      private string[] name = [];

      [Parameter(
        ParameterSetName = "CommonTCPPort",
        Position = 1,
        HelpMessage = "Specifies the common service TCP port number."
      )]
      [Alias("TCP")]
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
        get { return detailed; }
        set { detailed = value; }
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
        foreach (string n in name)
        {
          string p = string.Empty;
          ushort pn = 0;

          if (ParameterSetName == "RemotePort")
          {
            pn = port;
          }
          else
          {
            if (commonPort != string.Empty)
            {
              if (
                Enum.TryParse<TestHostWellKnownPort>(
                  commonPort,
                  true,
                  out var wellknownPort
                )
              )
              {
                p = wellknownPort.ToString();
              }
              else if (
                ushort.TryParse(
                  commonPort,
                  out var portNumber
                )
              )
              {
                pn = portNumber;
              }
            }
          }

          WriteTestNetConnection(
            n,
            p,
            pn
          );
        }
      }

      protected override void EndProcessing()
      {
        if (name.Length == 0)
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
} // namespace Browse
