namespace PowerModule.Commands;

abstract public partial class NativeCommand(bool SkipSsh = default) : CoreCommand(SkipSsh)
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

  abstract private protected string CommandPath { get; }

  abstract private protected string[] CommandScript { get; }

  virtual private protected SwitchBoard Uppercase { get; set; } = new();

  [Parameter(
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional arguments"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public string[] ArgumentList { private get; set; } = [];

  [Parameter(
    HelpMessage = "When execution results in a non-zero exit code, warn and continue instead of terminating execution"
  )]
  public SwitchParameter NoThrow
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -d flag as argument"
  )]
  public SwitchParameter D
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -e flag as argument"
  )]
  public SwitchParameter E
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as argument"
  )]
  public SwitchParameter I
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as argument"
  )]
  public SwitchParameter O
  {
    private protected get;
    set;
  }

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -p flag as argument"
  )]
  public SwitchParameter P
  {
    private protected get;
    set;
  }

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
      if (IsNativeArgument(argument))
      {
        NativeArguments.Add(argument);
      }
      else
      {
        Arguments.Add(argument);
      }
    }

    ArgumentList = [];

    PreprocessArguments();
  }

  sealed override private protected void Postprocess()
  {
    List<string> command = [
      "&",
      CommandPath,
      .. CommandScript,
    ];

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

    List<string> safeCommand = [];

    foreach (var word in command)
    {
      safeCommand.Add(
        Client.String.EscapeDoubleQuoted(
          word
        )
      );
    }

    AddScript(
      string.Join(
        Client.String.Space,
        safeCommand
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
}
