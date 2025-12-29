using System.Management.Automation;
using Completer.PathCompleter;

namespace Shell
{
  namespace Commands
  {
    [Cmdlet(
      VerbsCommon.Clear,
      "LineWip",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true
    )]
    [Alias("clt")]
    [OutputType(typeof(void))]
    public class ClearLine : PSCmdlet
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0
      )]
      [AllowEmptyString()]
      [SupportsWildcards()]
      [AllowNull()]
      public string Path
      {
        get => path;
        set => path = value;
      }
      private string path = string.Empty;

      [Parameter(
        Position = 1
      )]
      [SupportsWildcards()]
      public string Filter;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string[] LiteralPath;

      [Parameter()]
      [SupportsWildcards()]
      public string[] Include;

      [Parameter()]
      [SupportsWildcards()]
      public string[] Exclude;

      [Parameter()]
      [Alias("f")]
      public SwitchParameter Force;

      [Parameter()]
      public string Stream;

      private bool processRecords;

      protected override void BeginProcessing()
      {
        if (
          path != string.Empty
          || ParameterSetName == "LiteralPath"
        )
        {
          // $processRecords = $true
          processRecords = true;
          // $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand("Clear-Content", [System.Management.Automation.CommandTypes]::Cmdlet)

          // [scriptblock]$scriptCmd = { & $wrappedCmd @PSBoundParameters }

          // $steppablePipeline = $scriptCmd.GetSteppablePipeline()

          // $steppablePipeline.Begin($PSCmdlet)

        }
      }

      protected override void ProcessRecord()
      {
        if (processRecords)
        {
          // $steppablePipeline.Process($PSItem)
        }
      }

      protected override void EndProcessing()
      {
        if (processRecords)
        {
          // $steppablePipeline.End()
        }
        else
        {
          // Clear-Host
        }
      }

      private string Pwd() => this
        .SessionState
        .Path
        .CurrentLocation
        .Path;
    }
  }
} // namespace Shell
