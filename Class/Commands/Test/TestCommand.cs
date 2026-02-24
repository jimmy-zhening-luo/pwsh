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
  public string Name { get; set; } = "Hello, World";

  [Parameter]
  public SwitchParameter Switch
  {
    get => switchParameter;
    set => switchParameter = value;
  }
  private bool switchParameter;

  private protected sealed override void BeforeBeginProcessing()
  { }

  private protected sealed override void ProcessRecordAction()
  { }

  private protected sealed override void AfterEndProcessing()
  {
    WriteObject(
      Name
    );
  }
}
