namespace Module.PC.Environment;

internal static class Variable
{
  internal static bool Ssh => ssh ??= !string.IsNullOrEmpty(
    Env(
      "SSH_CLIENT"
    )
  );
  private static bool? ssh = null;
}
