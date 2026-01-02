using System;
using System.Management.Automation;

namespace Core
{
  namespace Windows
  {
    namespace Commands
    {
      [Cmdlet(
        VerbsData.Update,
        "Windows"
      )]
      [Alias("wu")]
      [OutputType(typeof(void))]
      public class UpdateWindows : PSCmdlet
      {
        private static bool Ssh() => Environment.GetEnvironmentVariable(
          "SSH_CLIENT"
        ) != null;

        protected override void EndProcessing()
        {
          if (!Ssh())
          {
            using Process proc = new()
            {
              StartInfo = new()
              {
                FileName = @"ms-settings:windowsupdate"
              }
            };
            proc.Start();
          }
        }
      }
    }
  } // namespace Windows
} // namespace Core
