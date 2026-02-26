namespace Module.Client.Environment.Known;

internal static class Variable
{
  internal static bool Ssh => ssh ??= !string.IsNullOrEmpty(
    Env.Get(
      "SSH_CLIENT"
    )
  );
  private static bool? ssh = default;
}
