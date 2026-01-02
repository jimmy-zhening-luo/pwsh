using System;
using System.Management.Automation;

namespace Core
{
  public class Command : Cmdlet, ICommand
  { }

  public class PSCommand : PSCmdlet, ICommand
  { }

  public interface ICommand
  {
    public bool Ssh() => Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;
  }
} // namespace Core
