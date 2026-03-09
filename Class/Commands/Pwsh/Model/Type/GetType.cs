namespace PowerModule.Commands.Pwsh.Model;

[Cmdlet(
  VerbsCommon.Get,
  "Type"
)]
[Alias("ty", "typeof")]
[OutputType(typeof(System.Reflection.TypeInfo))]
sealed public class GetInputType : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Object to type-check"
  )]
  [ValidateNotNull]
  required public object InputObject { get; set; }

  sealed override private protected void Process()
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
