namespace Module.Commands;

public abstract class NativeVerbCommand(
  string IntrinsicVerb = "",
  bool SkipSsh = default
) : NativeCommand(SkipSsh)
{
  private protected string IntrinsicVerb { get; set; } = IntrinsicVerb;

  private protected virtual void PreprocessArguments()
  { }

  private protected sealed override void Preprocess() => PreprocessArguments();

  private protected abstract List<string> NativeCommandArguments();

  private protected abstract List<string> NativeCommandVerbArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    var arguments = NativeCommandArguments();

    if (IntrinsicVerb is not "")
    {
      arguments.Add(IntrinsicVerb);
    }

    arguments.AddRange(NativeCommandVerbArguments());

    return arguments;
  }
}
