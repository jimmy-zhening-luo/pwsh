namespace Module.Commands;

abstract public class NativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeCommand(SkipSsh)
{
  private protected string? IntrinsicVerb { get; set; } = IntrinsicVerb;

  abstract private protected List<string> NativeCommandArguments { get; }

  abstract private protected List<string> NativeCommandVerbArguments { get; }

  sealed override private protected List<string> NativeCommandScript
  {
    get
    {
      List<string> arguments = [
        .. NativeCommandArguments,
      ];
  
      if (IntrinsicVerb is not null)
      {
        arguments.Add(IntrinsicVerb);
      }
  
      arguments.AddRange(
        NativeCommandVerbArguments
      );
  
      return arguments;
    }
  }
}
