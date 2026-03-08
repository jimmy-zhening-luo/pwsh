namespace Module.Commands;

abstract public class NativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeCommand(SkipSsh)
{
  private protected string? IntrinsicVerb { get; set; } = IntrinsicVerb;

  abstract private protected string[] CommandArguments { get; }

  abstract private protected string[] VerbArguments { get; }

  sealed override private protected string[] NativeCommandScript
  {
    get
    {
      List<string> arguments = [
        .. CommandArguments,
      ];

      if (IntrinsicVerb is not null)
      {
        arguments.Add(IntrinsicVerb);
      }

      arguments.AddRange(VerbArguments);

      return [.. arguments];
    }
  }
}
