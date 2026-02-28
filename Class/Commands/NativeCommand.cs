namespace Module.Commands;

public abstract class NativeCommand(
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  [Parameter(
    HelpMessage = "When execution results in a non-zero exit code, warn and continue instead of the default behavior of throwing a terminating error"
  )]
  public SwitchParameter NoThrow
  {
    get => noThrow;
    set => noThrow = value;
  }
  private protected bool noThrow;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -d flag as argument"
  )]
  public SwitchParameter D
  {
    get => d;
    set => d = value;
  }
  private protected bool d;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -E flag as argument"
  )]
  public SwitchParameter E
  {
    get => e;
    set => e = value;
  }
  private protected bool e;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as argument"
  )]
  public SwitchParameter I
  {
    get => i;
    set => i = value;
  }
  private protected bool i;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as argument"
  )]
  public SwitchParameter O
  {
    get => o;
    set => o = value;
  }
  private protected bool o;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -P flag as argument"
  )]
  public SwitchParameter P
  {
    get => p;
    set => p = value;
  }
  private protected bool p;
}
