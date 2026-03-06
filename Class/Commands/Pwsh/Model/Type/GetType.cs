namespace Module.Commands.Pwsh.Model.Type;

[Cmdlet(
  VerbsCommon.Get,
  "Type"
)]
[Alias("ty", "typeof")]
[OutputType(typeof(System.Reflection.TypeInfo))]
public sealed class GetType : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Object to type-check"
  )]
  [ValidateNotNull]
  public required object InputObject { get; set; }

  private protected sealed override void Process()
  {
    if (InputObject is PSObject inputPsObject)
    {
      WriteObject(
        inputPsObject
          .BaseObject
          .GetType()
      );
    }
    else
    {
      WriteObject(InputObject.GetType());
    }
  }
}
