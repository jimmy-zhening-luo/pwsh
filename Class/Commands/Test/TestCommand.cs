namespace PowerModule.Commands.Test;

[Cmdlet(
  VerbsDiagnostic.Test,
  "Command",
  SupportsShouldProcess = true
)]
[Alias("tt")]
[OutputType(typeof(object))]
sealed public class TestCommand : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromPipeline = true
  )]
  public Collection<string> Name
  { get; init; } = [];

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  public string Greeting
  { private get; init; } = "Hello";

  [Parameter]
  public SwitchParameter Switch
  { private get; init; }

  sealed override private protected void Preprocess()
  { }

  sealed override private protected void Process()
  {
    foreach (var name in Name)
    {
      WriteObject(Greet(name));
    }
  }

  sealed override private protected void Postprocess()
  {
    WriteObject($"The greeting was: {Greeting}");
    WriteObject($"The value of 'Switch' is: {Switch}");
  }

  string Greet(string name) => $"{Greeting}, {name}!";
}
