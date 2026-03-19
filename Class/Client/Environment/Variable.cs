namespace PowerModule.Client.Environment;

static class Variable
{
  static internal bool InSsh => ssh ??= Get("SSH_CLIENT") is not "";
  static bool? ssh;

  static internal string Get(string variable) => System.Environment.GetEnvironmentVariable(variable)
    ?? string.Empty;
}
