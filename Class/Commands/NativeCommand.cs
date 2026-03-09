namespace PowerModule.Commands;

abstract public partial class NativeCommand(
  string? IntrinsicVerb,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  sealed private protected record SwitchBoard(
    bool D = default,
    bool E = default,
    bool I = default,
    bool O = default,
    bool P = default,
    bool V = default
  );

  private protected readonly List<string> Arguments = [];
  private protected readonly List<string> NativeArguments = [];

  abstract private protected string CommandPath
  { get; }

  abstract private protected IEnumerable<string> CommandArguments
  { get; }

  abstract private protected IEnumerable<string> VerbArguments
  { get; }

  virtual private protected SwitchBoard Uppercase
  { get; init; } = new();

  [Parameter(
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public Collection<string> ArgumentList
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
      AddArgument(argument);
    }

    ArgumentList.Clear();

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
        Client.String.EscapeDoubleQuoted(
          word
        )
      );
    }

    AddScript(
      string.Join(
        Client.String.Space,
        safeCommandScript
      )
    );

    BeginSteppablePipeline();
    ProcessSteppablePipeline();
    EndSteppablePipeline();

    CheckNativeError(
      $"{CommandPath} error",
      !NoThrow
    );
  }

  private protected void AddArgument(string argument)
  {
    if (IsNativeArgument(argument))
    {
      NativeArguments.Add(argument);
    }
    else
    {
      Arguments.Add(argument);
    }
  }

  private protected void InsertArgument(string argument)
  {
    if (IsNativeArgument(argument))
    {
      NativeArguments.Insert(default, argument);
    }
    else
    {
      Arguments.Insert(default, argument);
    }
  }
}
