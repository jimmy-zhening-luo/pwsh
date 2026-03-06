namespace Module.Commands;

public abstract class NativeVerbCommand(
  string? IntrinsicVerb,
  bool CreateProcess = default,
  bool SkipSsh = default
) : NativeCommand(CreateProcess, SkipSsh)
{
  private protected string? IntrinsicVerb { get; set; } = IntrinsicVerb;

  private protected abstract List<string> NativeCommandArguments();

  private protected abstract List<string> NativeCommandVerbArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    var arguments = NativeCommandArguments();

    if (IntrinsicVerb is not null)
    {
      arguments.Add(IntrinsicVerb);
    }

    arguments.AddRange(NativeCommandVerbArguments());

    return arguments;
  }
}
