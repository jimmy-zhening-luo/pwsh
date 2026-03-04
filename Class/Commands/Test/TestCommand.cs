namespace Module.Commands.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Command"
)]
[Alias("tt")]
[OutputType(typeof(object))]
public sealed class TestCommand : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromPipeline = true
  )]
  public string[] Name { get; set; } = [];

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  public string Greeting { get; set; } = "Hello";

  [Parameter]
  public SwitchParameter Switch { get; set; }

  private protected sealed override void Preprocess()
  { }

  private protected sealed override void Process()
  {
    foreach (var name in Name)
    {
      WriteObject(Greet(name));
    }
  }

  private protected sealed override void Postprocess()
  {
    WriteObject($"The greeting was: {Greeting}");
    WriteObject($"The value of 'Switch' is: {Switch}");

    if (!Switch)
    {
      WriteObject("Switch evaluates to false.");
    }
    else
    {
      WriteObject("Switch evaluates to true.");
    }
  }

  private string Greet(string name) => $"{Greeting}, {Name}!";
}
