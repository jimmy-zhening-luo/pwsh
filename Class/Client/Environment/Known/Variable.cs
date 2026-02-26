namespace Module.Client.Environment.Known;

internal static class Variable
{
  internal static bool InSsh => inSsh
    ??= Env.Get("SSH_CLIENT") is not (null or "");
  private static bool? inSsh = default;
}
