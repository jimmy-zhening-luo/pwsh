using System;
using System.Management.Automation;

namespace Core
{
  public abstract class Command : Cmdlet, ICommand
  { }

  public abstract class PSCommand : PSCmdlet, ICommand
  { }

  public interface ICommand
  {
    public bool Ssh() => Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;
  }
} // namespace Core
