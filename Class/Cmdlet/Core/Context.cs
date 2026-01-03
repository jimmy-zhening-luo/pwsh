namespace Core
{
  public static class Context
  {
    public static bool Ssh() => System.Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;
  }
}
