namespace Module.Client.Environment.Known;

internal static class Variable
{
  internal static bool Ssh => ssh ??= Env.Get(
    "SSH_CLIENT"
  )
    is not null
    and not "";
  private static bool? ssh = default;
}
