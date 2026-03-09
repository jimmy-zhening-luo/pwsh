namespace PowerModule.Client.Environment.Known;

static internal class Variable
{
  static internal bool InSsh => ssh ??= Local.Get("SSH_CLIENT") is not "";
  static private bool? ssh;
}
