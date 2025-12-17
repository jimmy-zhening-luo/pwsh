using System;
using System.Management.Automation;

namespace PathCompleter
{
  public enum PathProvider
  {
    Any,
    FileSystem,
    Registry,
    Environment,
    Variable,
    Alias,
    Function
  }

  public enum PathItemType
  {
    Any,
    File,
    Directory,
    RegistryKey,
    EnvironmentVariable,
    Variable,
    Alias,
    Function
  }
}
