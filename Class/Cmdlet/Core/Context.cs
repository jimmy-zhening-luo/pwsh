using System;

namespace Core
{
  public static class Context
  {
    public static bool Ssh() => Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;
  }
} // namespace Core
