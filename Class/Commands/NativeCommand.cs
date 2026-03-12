namespace PowerModule.Commands;

abstract public partial class NativeCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected record struct SwitchBoard(
    bool D = default,
    bool E = default,
    bool I = default,
    bool O = default,
    bool P = default,
    bool V = default
  );

  private protected readonly LinkedList<string> Arguments = [];
  private protected readonly LinkedList<string> NativeArguments = [];

  abstract private protected string CommandPath
  { get; }

  abstract private protected IEnumerable<string> CommandArguments
  { get; }

  abstract private protected IEnumerable<string> VerbArguments
  { get; }

  virtual private protected SwitchBoard Uppercase
  { get; }

  [Parameter(
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public string[] ArgumentList
  { private get; init; } = [];

  [Parameter(
    HelpMessage = "When execution results in a non-zero exit code, warn and continue instead of terminating execution"
  )]
  public SwitchParameter NoThrow
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -d flag as argument"
  )]
  public SwitchParameter D
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -e flag as argument"
  )]
  public SwitchParameter E
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as argument"
  )]
  public SwitchParameter I
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as argument"
  )]
  public SwitchParameter O
  { private protected get; set; }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -p flag as argument"
  )]
  public SwitchParameter P
  { private protected get; set; }

  private protected string? IntrinsicVerb
  { get; set; } = IntrinsicVerb;

  static private protected bool IsNativeArgument(string argument) => NativeArgumentRegex().IsMatch(argument);
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?>-(?>[A-Za-z]|(?>(?>-[A-Za-z][A-Za-z\d]*(?>_[A-Za-z\d]+)*)(?>-[A-Za-z\d]+(?>_[A-Za-z\d]+)*)*)))(?>=\S+)?$"
  )]
  static private partial System.Text.RegularExpressions.Regex NativeArgumentRegex();

  virtual private protected void PreprocessArguments()
  { }

  sealed override private protected void Preprocess()
  {
    foreach (var argument in ArgumentList)
    {
      WriteDebug(argument);

      AddLast(argument);
    }

    PreprocessArguments();
  }

  sealed override private protected void Postprocess()
  {
    List<string> commandScript = [
      "&",
      CommandPath,
      .. CommandArguments,
    ];

    if (IntrinsicVerb is not null)
    {
      commandScript.Add(IntrinsicVerb);
    }

    commandScript.AddRange(VerbArguments);

    if (D)
    {
      commandScript.Add(Uppercase.D ? "-D" : "-d");
    }

    if (E)
    {
      commandScript.Add(Uppercase.E ? "-E" : "-e");
    }

    if (I)
    {
      commandScript.Add(Uppercase.I ? "-I" : "-i");
    }

    if (O)
    {
      commandScript.Add(Uppercase.O ? "-O" : "-o");
    }

    if (P)
    {
      commandScript.Add(Uppercase.P ? "-P" : "-p");
    }

    if (V)
    {
      commandScript.Add(Uppercase.V ? "-V" : "-v");
    }

    commandScript.AddRange(Arguments);
    commandScript.AddRange(NativeArguments);

    List<string> safeCommandScript = [];

    foreach (var word in commandScript)
    {
      safeCommandScript.Add(
        Client.StringInput.EscapeDoubleQuoted(
          word
        )
      );
    }

    var safeCommandScriptString = string.Join(
      Client.StringInput.Space,
      safeCommandScript
    );

    WriteDebug(safeCommandScriptString);

    _ = AddScript(safeCommandScriptString);

    BeginSteppablePipeline();
    ProcessSteppablePipeline();
    EndSteppablePipeline();

    CheckNativeError(
      $"{CommandPath} error",
      !NoThrow
    );
  }

  private protected void AddFirst(string argument) => _ = IsNativeArgument(argument)
    ? NativeArguments.AddFirst(argument)
    : Arguments.AddFirst(argument);

  private protected void AddLast(string argument) => _ = IsNativeArgument(argument)
    ? NativeArguments.AddLast(argument)
    : Arguments.AddLast(argument);
}
