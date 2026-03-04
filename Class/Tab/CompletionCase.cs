namespace Module.Tab;

internal enum CompletionCase
{
  [System.ComponentModel.Description("Preserve the original case of the matched completion")]
  Preserve,

  [System.ComponentModel.Description("Convert the matched completion to lowercase")]
  Lower,

  [System.ComponentModel.Description("Convert the matched completion to uppercase")]
  Upper,
}
