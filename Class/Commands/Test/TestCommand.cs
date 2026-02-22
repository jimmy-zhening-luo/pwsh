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
    Position = 0
  )]
  public string Name
  {
    get => name;
    set => name = value;
  }
  private string name = "Hello, World";

  private protected sealed override void AfterEndProcessing()
  {
    WriteObject(
      name
    );
  }
}
