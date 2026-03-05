namespace Module.Client.Environment.Known;

internal static class Variable
{
  internal static bool InSsh => ssh ??= Local.Get("SSH_CLIENT") is not (null or "");
  private static bool? ssh;
}
