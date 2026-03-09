namespace PowerModule.Commands;

abstract public class NativeVerbCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : NativeCommand(SkipSsh)
{
  private protected string? IntrinsicVerb
  { get; set; } = IntrinsicVerb;

  abstract private protected IEnumerable<string> CommandArguments
  { get; }

  abstract private protected IEnumerable<string> VerbArguments
  { get; }

  sealed override private protected IEnumerable<string> CommandScript => IntrinsicVerb is null
    ? [
        .. CommandArguments,
        .. VerbArguments
      ]
    : [
        .. CommandArguments,
        IntrinsicVerb,
        .. VerbArguments
      ];
}
