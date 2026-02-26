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
    Position = 0,
    ValueFromPipeline = true
  )]
  public string[] Name { get; set; } = [];

  [Parameter(
    Position = 1
  )]
  [ValidateNotNullOrWhiteSpace]
  public string Greeting { get; set; } = "Hello";

  [Parameter]
  public SwitchParameter Switch
  {
    get => switchParameter;
    set => switchParameter = value;
  }
  private bool switchParameter;

  private protected sealed override void Preprocess()
  { }

  private protected sealed override void Processor()
  {
    foreach (var name in Name)
    {
      WriteObject(
        Greet(
          name
        )
      );
    }
  }

  private protected sealed override void Postprocess()
  {
    WriteObject(
      string.Concat(
        "The greeting was: ",
        Greeting
      )
    );
  }

  private string Greet(
    string name
  ) => string.Concat(
    Greeting,
    ", ",
    name,
    "!"
  );
}
