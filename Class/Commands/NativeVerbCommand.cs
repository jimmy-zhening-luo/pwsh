namespace Module.Commands;

abstract public class NativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeCommand(SkipSsh)
{
  private protected string? IntrinsicVerb { get; set; } = IntrinsicVerb;

  abstract private protected List<string> NativeCommandArguments();

  abstract private protected List<string> NativeCommandVerbArguments();

  sealed override private protected List<string> BuildNativeCommand()
  {
    var arguments = NativeCommandArguments();

    if (IntrinsicVerb is not null)
    {
      arguments.Add(IntrinsicVerb);
    }

    arguments.AddRange(
      NativeCommandVerbArguments()
    );

    return arguments;
  }
}
