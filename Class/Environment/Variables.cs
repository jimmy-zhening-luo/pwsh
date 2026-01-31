namespace Module.Environment;

internal static partial class Environmental
{
  internal static bool Ssh => ssh ??= !string.IsNullOrEmpty(
    Env(
      "SSH_CLIENT"
    )
  );
  private static bool? ssh = null;
}
