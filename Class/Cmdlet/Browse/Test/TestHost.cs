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
      "HostWip",
      DefaultParameterSetName = "CommonTCPPort",
      HelpUri = "https://learn.microsoft.com/powershell/module/nettcpip/test-netconnection"
    )]
    // [Alias("tn")]
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

      public SwitchParameter Detailed
      {
        get { return detailed; }
        set { detailed = value; }
      }
      private bool detailed;

      protected override void ProcessRecord()
      {
        return;
      }

      protected override void EndProcessing()
      {
        return;
      }
    }
  }
} // namespace Browse
