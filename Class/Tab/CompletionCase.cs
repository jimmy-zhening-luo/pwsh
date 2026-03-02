namespace Module.Tab;

internal enum CompletionCase
{
  [System.ComponentModel.Description("Preserve the original case of the matched completion item")]
  Preserve,

  [System.ComponentModel.Description("Change the matched completion item to lowercase")]
  Lower,

  [System.ComponentModel.Description("Change the matched completion item to uppercase")]
  Upper,
}
