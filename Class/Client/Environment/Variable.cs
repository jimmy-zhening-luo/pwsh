namespace PowerModule.Client.Environment;

static class Variable
{
  static internal string Get(string variable) => System.Environment.GetEnvironmentVariable(variable)
    ?? string.Empty;

  static internal bool InSsh => ssh ??= Get("SSH_CLIENT") is not "";
  static private bool? ssh;
}
