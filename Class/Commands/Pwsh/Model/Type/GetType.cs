namespace PowerModule.Commands.Pwsh.Model.Type;

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
  required public object InputObject
  { get; init; }

  sealed override private protected void Process() => WriteObject(
    (
      InputObject is PSObject inputPsObject
        ? inputPsObject.BaseObject
        : InputObject
    )
      .GetType()
  );
}
