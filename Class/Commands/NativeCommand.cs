namespace PowerModule.Commands;

abstract public partial class NativeCommand(
  string CommandPath,
  string? IntrinsicVerb,
  string[]? CommandArgumentBase = default,
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
    HelpMessage = "If an error is encountered, warn and continue instead of terminating execution"
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
    @"^(?>-(?>[A-Za-z]|(?>-[^\W_]+(?>[-_][^\W_]+)*)))(?>=(?>""(?>\\\S|[ \S-[\\""]])*""|[^\s""]+))?$"
  )]
  static private partial System.Text.RegularExpressions.Regex NativeArgumentRegex();

  virtual private protected void PreprocessArguments()
  { }

  virtual private protected string[] ParseRuntimeCommandArguments() => [];

  virtual private protected string[] ParseRuntimeVerbArguments() => [];

  sealed override private protected void Preprocess()
  {
    foreach (var argument in ArgumentList)
    {
      AddLast(argument);
    }

    PreprocessArguments();
  }

  sealed override private protected void Postprocess()
  {
    List<string> command = [
      "&",
      CommandPath,
    ];

    if (CommandArgumentBase is not null)
    {
      command.AddRange(
        CommandArgumentBase
      );
    }

    command.AddRange(
      ParseRuntimeCommandArguments()
    );

    if (IntrinsicVerb is not null)
    {
      command.Add(
        IntrinsicVerb
      );
    }

    command.AddRange(
      ParseRuntimeVerbArguments()
    );

    if (D)
    {
      command.Add(Uppercase.D ? "-D" : "-d");
    }

    if (E)
    {
      command.Add(Uppercase.E ? "-E" : "-e");
    }

    if (I)
    {
      command.Add(Uppercase.I ? "-I" : "-i");
    }

    if (O)
    {
      command.Add(Uppercase.O ? "-O" : "-o");
    }

    if (P)
    {
      command.Add(Uppercase.P ? "-P" : "-p");
    }

    if (V)
    {
      command.Add(Uppercase.V ? "-V" : "-v");
    }

    command.AddRange(Arguments);
    command.AddRange(NativeArguments);

    List<string> escapedCommand = [];

    foreach (var word in command)
    {
      escapedCommand.Add(
        Client.StringInput.EscapeDoubleQuoted(
          word
        )
      );
    }

    var commandScript = string.Join(
      Client.StringInput.Space,
      escapedCommand
    );

    WriteDebug(commandScript);

    ClearCommands();

    _ = AddScript(commandScript);

    BeginSteppablePipeline();
    ProcessSteppablePipeline();
    EndSteppablePipeline();

    CheckNativeError(
      $"{CommandPath} error",
      !NoThrow
    );
  }

  virtual private protected void ClearArguments()
  {
    (
      D,
      E,
      I,
      O,
      P,
      V
    ) = (
      default,
      default,
      default,
      default,
      default,
      default
    );

    Arguments.Clear();
    NativeArguments.Clear();
  }

  private protected void AddFirst(string argument) => _ = IsNativeArgument(argument)
    ? NativeArguments.AddFirst(argument)
    : Arguments.AddFirst(argument);

  private protected void AddLast(string argument) => _ = IsNativeArgument(argument)
    ? NativeArguments.AddLast(argument)
    : Arguments.AddLast(argument);
}
